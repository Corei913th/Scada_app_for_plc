using System.Text.Json;

namespace Scada_App_For_plc;

/// <summary>
/// Paramètres de communication avec l'automate (section « Plc » de appsettings.json).
/// Les numéros de relais / registres correspondent à ceux du programme Ladder (ISPSoft).
/// </summary>
public sealed class PlcSettings
{
    /// <summary>Adresse IP de l'automate (module Ethernet) ; le PC doit être sur le même sous-réseau.</summary>
    public string Ip { get; set; } = "192.168.0.1";

    /// <summary>Port Modbus TCP standard.</summary>
    public int Port { get; set; } = 502;

    /// <summary>
    /// Façon d'écrire les messages Modbus :
    ///   "Tcp"          = Modbus TCP classique (vrai automate AS / DVP-ES sur Ethernet, port 502, PlcSimulator) ;
    ///   "AsciiOverTcp" = Modbus ASCII transporté sur TCP (simulateur « DVP Simulator » de COMMGR / ISPSoft, port 10002).
    /// </summary>
    public string Protocol { get; set; } = "Tcp";

    /// <summary>Identifiant d'esclave Modbus (« Slave ID ») de l'automate.</summary>
    public byte UnitId { get; set; } = 1;

    /// <summary>
    /// Adresse Modbus de M0. Les moteurs utilisent M0..M5.
    /// Elle dépend de la gamme d'automate Delta (ex. série DVP : 0x0800 = 2048) : voir son manuel,
    /// tableau « correspondance des adresses Modbus ». 0 si l'automate numérote ses bobines depuis M0.
    /// </summary>
    public ushort CoilOffset { get; set; } = 0;

    /// <summary>Adresse Modbus de D0 (ex. série DVP : 0x1000 = 4096). Même remarque que CoilOffset.</summary>
    public ushort RegisterOffset { get; set; } = 0;

    /// <summary>Numéro du relais M qui autorise la marche machine (M20) : c'est le « bouton Start » du Ladder.</summary>
    public ushort StartCoil { get; set; } = 20;

    /// <summary>Premier registre D de consigne (D3000..D3002), écrit par le PC, lu par le programme Ladder.</summary>
    public ushort SetpointStart { get; set; } = 3000;

    /// <summary>Premier registre D de mesure (D3100..D3102), écrit par le programme Ladder, lu par le PC.</summary>
    public ushort FeedbackStart { get; set; } = 3100;

    /// <summary>Période de rafraîchissement de l'écran principal, en ms (lecture cyclique des relais M).</summary>
    public int PollMs { get; set; } = 100;
}

/// <summary>Section « Database » de appsettings.json.</summary>
public sealed class DatabaseSettings
{
    /// <summary>Chaîne de connexion PostgreSQL (hôte, port, base, utilisateur, mot de passe).</summary>
    public string ConnectionString { get; set; } =
        "Host=localhost;Port=5432;Database=scada;Username=postgres;Password=CHANGE_ME";
}

/// <summary>Configuration complète, chargée au démarrage depuis appsettings.json.</summary>
public sealed class AppSettings
{
    public PlcSettings Plc { get; set; } = new();
    public DatabaseSettings Database { get; set; } = new();

    /// <summary>
    /// Lit appsettings.json (copié à côté de l'exécutable). Si le fichier est absent,
    /// les valeurs par défaut ci-dessus sont utilisées.
    /// </summary>
    public static AppSettings Load()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
            return new AppSettings();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,   // autorise les // commentaires dans le JSON
            AllowTrailingCommas = true
        };
        return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path), options) ?? new AppSettings();
    }
}
