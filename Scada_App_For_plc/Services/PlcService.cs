using System.Net.Sockets;
using NModbus;
using NModbus.IO;

namespace Scada_App_For_plc.Services;

/// <summary>Sens de rotation d'un moteur, déduit des deux relais M (avant / arrière) de l'automate.</summary>
public enum MotorDirection
{
    Stopped,
    Forward,
    Backward,
    /// <summary>Avant et arrière actifs en même temps (état incohérent côté automate).</summary>
    Conflict
}

/// <summary>Photo de l'état de la machine lue dans l'automate.</summary>
/// <param name="MachineRunning">État du relais de marche machine (M20 par défaut).</param>
/// <param name="Motors">Sens de chaque moteur, indice 0 = moteur 1.</param>
public sealed record PlcStatus(bool MachineRunning, MotorDirection[] Motors);

/// <summary>
/// Dialogue Modbus TCP avec l'automate Delta (le PC est le « maître », l'automate l'« esclave »).
///
/// Rappel des types de données Modbus utilisés ici, et de leur équivalent côté automate Delta :
///   - Bobine (coil)           : 1 bit lecture/écriture   ↔ relais auxiliaire M (M0, M1, ... M20)
///   - Registre de maintien    : mot de 16 bits (0..65535) ↔ registre de données D (D3000, D3100, ...)
///
/// Codes fonction Modbus employés (gérés par la bibliothèque NModbus) :
///   FC01 lire des bobines  |  FC05 écrire une bobine  |  FC03 lire des registres  |  FC06 écrire un registre
///
/// Toutes les opérations sont sérialisées (un seul échange à la fois sur la socket),
/// car le timer de l'écran principal et les boutons peuvent appeler le service en même temps.
/// </summary>
public sealed class PlcService : IDisposable
{
    /// <summary>Nombre de moteurs pilotés (M0..M5 pour les sens, D3000.. / D3100.. pour les vitesses).</summary>
    public const int MotorCount = 3;

    private readonly PlcSettings _s;

    // Verrou qui garantit une seule requête Modbus à la fois sur la connexion.
    private readonly SemaphoreSlim _lock = new(1, 1);
    private TcpClient? _tcp;
    private IModbusMaster? _master;
    private bool _connecting;

    public PlcService(PlcSettings settings) => _s = settings;

    /// <summary>Vrai si la liaison TCP avec l'automate est ouverte.</summary>
    public bool IsConnected => _master is not null && _tcp?.Connected == true;

    /// <summary>Levé à chaque ouverture ou perte de liaison (la fenêtre principale met son voyant à jour).</summary>
    public event Action? ConnectionChanged;

    // ---------------------------------------------------------------- connexion

    /// <summary>Ouvre la liaison TCP vers l'IP / le port Modbus de l'automate (502 par défaut).</summary>
    public async Task ConnectAsync(CancellationToken ct = default)
    {
        // Évite deux tentatives simultanées (le timer de reconnexion tourne toutes les 3 s).
        if (_connecting || IsConnected)
            return;

        _connecting = true;
        try
        {
            Close();
            var tcp = new TcpClient();

            // Automate éteint ou câble débranché : on abandonne après 2 s au lieu de bloquer l'écran.
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            await tcp.ConnectAsync(_s.Ip, _s.Port, cts.Token);

            // Le « master » NModbus fabrique et décode les trames. Deux formats possibles (voir PlcSettings.Protocol) :
            //   Modbus TCP   : trames binaires avec en-tête MBAP (vrai automate Ethernet) ;
            //   Modbus ASCII : trames en texte « :0103... » + somme de contrôle LRC (simulateur COMMGR).
            var factory = new ModbusFactory();
            var master = string.Equals(_s.Protocol, "AsciiOverTcp", StringComparison.OrdinalIgnoreCase)
                ? factory.CreateAsciiMaster(new TcpClientAdapter(tcp))
                : factory.CreateMaster(tcp);
            master.Transport.ReadTimeout = 1000;   // ms : l'automate doit répondre en moins d'1 s
            master.Transport.WriteTimeout = 1000;
            master.Transport.Retries = 0;          // pas de ré-essai : on préfère détecter vite la panne

            _tcp = tcp;
            _master = master;
            ConnectionChanged?.Invoke();
        }
        finally
        {
            _connecting = false;
        }
    }

    public void Close()
    {
        // On se base sur _master : TcpClient.Connected peut déjà être faux après une coupure.
        var wasConnected = _master is not null;
        _master?.Dispose();
        _tcp?.Dispose();
        _master = null;
        _tcp = null;
        if (wasConnected)
            ConnectionChanged?.Invoke();
    }

    public void Dispose()
    {
        Close();
        _lock.Dispose();
    }

    // ------------------------------------------------------- bobines (M0..M20)

    /// <summary>
    /// Convertit un numéro de relais M en adresse Modbus de bobine.
    /// CoilOffset = adresse Modbus de M0 : elle dépend du modèle d'automate
    /// (ex. série DVP : M0 = 0x0800 = 2048) → à régler dans appsettings.json.
    /// </summary>
    private ushort Coil(int m) => (ushort)(_s.CoilOffset + m);

    /// <summary>
    /// Lit d'un seul coup les relais M0..M20 (FC01), puis en déduit l'état des moteurs.
    /// Convention du programme Ladder : moteur n (0..2) → avant = M(2n), arrière = M(2n+1) ;
    /// donc M0/M1 = moteur 1, M2/M3 = moteur 2, M4/M5 = moteur 3. M20 = marche machine.
    /// </summary>
    public async Task<PlcStatus> ReadStatusAsync()
    {
        // On lit à partir de M0 sur (StartCoil + 1) bobines : le tableau est donc indexé par le n° de M.
        var coils = await RunAsync(m => m.ReadCoilsAsync(_s.UnitId, Coil(0), (ushort)(_s.StartCoil + 1)));

        var motors = new MotorDirection[MotorCount];
        for (var i = 0; i < MotorCount; i++)
        {
            var fwd = coils[2 * i];       // M(2n)   : marche avant
            var bwd = coils[2 * i + 1];   // M(2n+1) : marche arrière
            motors[i] = (fwd, bwd) switch
            {
                (true, true) => MotorDirection.Conflict,   // ne devrait jamais arriver (verrouillage Ladder)
                (true, false) => MotorDirection.Forward,
                (false, true) => MotorDirection.Backward,
                _ => MotorDirection.Stopped
            };
        }

        return new PlcStatus(coils[_s.StartCoil], motors);
    }

    /// <summary>Écrit le relais de marche machine (M20, FC05) : true = marche, false = arrêt général.</summary>
    public Task SetMachineRunningAsync(bool running) =>
        RunAsync(async m =>
        {
            await m.WriteSingleCoilAsync(_s.UnitId, Coil(_s.StartCoil), running);
            return 0;
        });

    /// <summary>Commande le sens d'un moteur (0..2) en positionnant ses deux relais M avant / arrière.</summary>
    public Task SetMotorAsync(int motor, MotorDirection direction) =>
        RunAsync(async m =>
        {
            var fwd = Coil(2 * motor);
            var bwd = Coil(2 * motor + 1);

            // On coupe d'abord le sens opposé : jamais les deux sens à la fois.
            switch (direction)
            {
                case MotorDirection.Forward:
                    await m.WriteSingleCoilAsync(_s.UnitId, bwd, false);
                    await m.WriteSingleCoilAsync(_s.UnitId, fwd, true);
                    break;
                case MotorDirection.Backward:
                    await m.WriteSingleCoilAsync(_s.UnitId, fwd, false);
                    await m.WriteSingleCoilAsync(_s.UnitId, bwd, true);
                    break;
                default: // arrêt : les deux relais à 0
                    await m.WriteSingleCoilAsync(_s.UnitId, fwd, false);
                    await m.WriteSingleCoilAsync(_s.UnitId, bwd, false);
                    break;
            }

            return 0;
        });

    // ------------------------------------------------------ registres (D3000..)

    /// <summary>
    /// Convertit un numéro de registre D en adresse Modbus de registre.
    /// RegisterOffset = adresse Modbus de D0 (ex. série DVP : D0 = 0x1000 = 4096).
    /// </summary>
    private ushort Reg(int d) => (ushort)(_s.RegisterOffset + d);

    /// <summary>Consignes de vitesse actuellement dans l'automate (D3000..D3002, écrites par le PC).</summary>
    public async Task<int[]> ReadSetpointsAsync() => await ReadRegistersAsync(_s.SetpointStart);

    /// <summary>Vitesses mesurées par l'automate (D3100..D3102, calculées par le programme Ladder).</summary>
    public async Task<int[]> ReadFeedbackAsync() => await ReadRegistersAsync(_s.FeedbackStart);

    /// <summary>Lit MotorCount registres D consécutifs à partir de D<paramref name="firstD"/> (FC03).</summary>
    private async Task<int[]> ReadRegistersAsync(int firstD)
    {
        var regs = await RunAsync(m => m.ReadHoldingRegistersAsync(_s.UnitId, Reg(firstD), MotorCount));
        return regs.Select(r => (int)r).ToArray();
    }

    /// <summary>Écrit les consignes dans D3000.. (FC06, un registre à la fois). Le registre est un mot de 16 bits non signé.</summary>
    public Task WriteSetpointsAsync(IReadOnlyList<int> values) =>
        RunAsync(async m =>
        {
            for (var i = 0; i < values.Count; i++)
            {
                // Un registre D ne peut contenir que 0..65535 : on borne pour ne jamais déborder.
                var v = (ushort)Math.Clamp(values[i], 0, ushort.MaxValue);
                await m.WriteSingleRegisterAsync(_s.UnitId, Reg(_s.SetpointStart + i), v);
            }

            return 0;
        });

    // ------------------------------------------------------------------ interne

    /// <summary>
    /// Exécute une opération Modbus en exclusion mutuelle et transforme toute coupure réseau
    /// en une erreur lisible. Après une coupure la liaison est fermée : la fenêtre principale
    /// retentera la connexion automatiquement.
    /// </summary>
    private async Task<T> RunAsync<T>(Func<IModbusMaster, Task<T>> operation)
    {
        await _lock.WaitAsync();
        try
        {
            var master = _master ?? throw new InvalidOperationException("Automate non connecté.");
            return await operation(master);
        }
        catch (Exception ex) when (ex is IOException or SocketException or TimeoutException or ObjectDisposedException)
        {
            // Perte de liaison : on referme, le shell retentera la connexion.
            Close();
            throw new InvalidOperationException("Liaison avec l'automate perdue : " + ex.Message, ex);
        }
        finally
        {
            _lock.Release();
        }
    }
}
