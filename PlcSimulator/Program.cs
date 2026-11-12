// Simulateur d'automate Delta pour tester le SCADA SANS automate réel.
//
// Le simulateur de COMMGR / ISPSoft (« DVP Simulator », port 10002) n'est visible que d'ISPSoft :
// il n'a pas d'adresse IP et ne parle pas Modbus TCP. Ce petit programme joue donc le rôle de
// l'automate côté réseau : il ouvre un serveur Modbus TCP sur ce PC et reproduit la logique Ladder
// attendue par le SCADA (mêmes relais M et mêmes registres D que dans appsettings.json).
//
// La console affiche : un lexique, l'état en direct de l'automate, et un JOURNAL qui dit en français
// ce que le SCADA vient d'écrire et ce que le « Ladder » simulé en déduit.
//
// Utilisation :  dotnet run --project PlcSimulator [port]      (port 502 par défaut)
// Côté SCADA, dans appsettings.json :  "Ip": "127.0.0.1", "Port": 502, offsets à 0.

using System.Net;
using System.Text;
using NModbus;

Console.OutputEncoding = Encoding.UTF8;

var port = args.Length > 0 && int.TryParse(args[0], out var p) ? p : 502;

// ---- Adresses (identiques à appsettings.json) ----
const int StartCoil = 20;        // M20 : marche machine
const int SetpointStart = 3000;  // D3000..D3002 : consignes écrites par le PC
const int FeedbackStart = 3100;  // D3100..D3102 : vitesses « mesurées » rendues au PC
const int Motors = 3;            // moteur n : avant = M(2n), arrière = M(2n+1)
const double Gain = 0.15;        // part de l'écart rattrapée à chaque scan (inertie du moteur)
const int ScanMs = 50;           // durée d'un cycle automate simulé

// ---- Serveur Modbus TCP (le simulateur est l'esclave, unité 1 comme l'automate) ----
var factory = new ModbusFactory();
var listener = new System.Net.Sockets.TcpListener(IPAddress.Loopback, port);
try
{
    listener.Start();
}
catch (Exception ex)
{
    Console.WriteLine($"Impossible d'ouvrir le port {port} : {ex.Message}");
    Console.WriteLine("Un autre programme l'utilise peut-être ; essaie un autre port : dotnet run -- 5020");
    return;
}

var network = factory.CreateSlaveNetwork(listener);
var slave = factory.CreateSlave(1);
network.AddSlave(slave);
_ = Task.Run(() => network.ListenAsync());

var coils = slave.DataStore.CoilDiscretes;       // les relais M
var regs = slave.DataStore.HoldingRegisters;     // les registres D

// Consignes de départ, comme si le Ladder en avait préchargé dans les D.
regs.WritePoints(SetpointStart, [1000, 1500, 2000]);

var speed = new double[Motors];                  // vitesse simulée de chaque moteur (avec inertie)
var reached = new bool[Motors];                  // la vitesse a-t-elle rejoint la cible ? (pour le journal)
var wasRunning = new bool[Motors];               // le moteur tournait-il au scan précédent ?

// « Photo » de l'état à la fin du scan précédent : toute différence au scan suivant vient du SCADA.
var prevRun = false;
var prevFwd = new bool[Motors];
var prevBwd = new bool[Motors];
var prevSet = new ushort[] { 1000, 1500, 2000 };

// ---- Journal : les dernières lignes affichées en bas de l'écran ----
const int LogLines = 9;
var redirected = Console.IsOutputRedirected;     // sortie vers un fichier : pas d'écran, on écrit le journal au fil de l'eau
var log = new List<string>();
void Log(string text)
{
    var line = $"{DateTime.Now:HH:mm:ss}  {text}";
    log.Add(line);
    if (log.Count > LogLines) log.RemoveAt(0);
    if (redirected) Console.WriteLine(line);
}

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) => { e.Cancel = true; cts.Cancel(); };

// ---- Affichage : le lexique est écrit une fois, le reste est réécrit à chaque scan ----
string[] legend =
[
    $"Simulateur d'automate Delta - Modbus TCP sur 127.0.0.1:{port} (Ctrl+C pour quitter)",
    new string('=', 100),
    "LEXIQUE",
    " M  = relais interne de l'automate : un bit, 0 ou 1.      D = registre de données : un nombre de 0 à 65535.",
    $" M{StartCoil}  = marche machine (boutons DÉMARRER / ARRÊTER du SCADA). À 0, l'automate coupe tous les moteurs.",
    " M0 M1 | M2 M3 | M4 M5 = sens des moteurs 1, 2, 3 : le premier est AVANT, le second ARRIÈRE (jamais les deux à 1).",
    $" Consigne (D{SetpointStart}..) = vitesse DEMANDÉE : écrite par le SCADA (page Paramètres, bouton « Appliquer »).",
    $" Mesure   (D{FeedbackStart}..) = vitesse RÉELLE simulée : lue par le SCADA. Recalculée à chaque scan ({ScanMs} ms) :",
    $"      mesure = mesure + (cible - mesure) x {Gain:0.00}      avec  cible = consigne si le moteur tourne, 0 sinon",
    "      -> la vitesse monte (ou redescend) en douceur, comme un vrai moteur qui a de l'inertie.",
    new string('=', 100),
    "ÉTAT EN DIRECT DE L'AUTOMATE",
];
var liveRow = legend.Length;                     // première ligne de la zone « en direct »
var logRow = liveRow + 2 + Motors;               // ligne du titre du journal (les entrées suivent)

int Width() { try { return Math.Max(40, Console.WindowWidth - 1); } catch { return 119; } }

void WriteAt(int row, string text)
{
    if (redirected) return;
    try
    {
        Console.SetCursorPosition(0, row);
        var w = Width();
        Console.Write((text.Length > w ? text[..w] : text).PadRight(w));
    }
    catch
    {
        // Fenêtre trop petite ou console redirigée : on ignore l'affichage.
    }
}

try { Console.Clear(); } catch { }
for (var i = 0; i < legend.Length; i++) WriteAt(i, legend[i]);
Log("Simulateur démarré, en attente du SCADA (les lectures du SCADA toutes les 100 ms ne sont pas journalisées).");

string Bar(double value, double max)
{
    var ratio = max <= 0 ? 0 : Math.Clamp(value / max, 0, 1);
    var filled = (int)Math.Round(ratio * 20);
    return "[" + new string('█', filled) + new string('░', 20 - filled) + $"] {ratio * 100,3:0}%";
}

string[] MotorWord = ["moteur 1", "moteur 2", "moteur 3"];

// ---- Cycle automate : équivalent d'un « scan » Ladder toutes les 50 ms ----
while (!cts.IsCancellationRequested)
{
    // 1) Entrées : ce que le SCADA a écrit depuis le scan précédent.
    var run = coils.ReadPoints(StartCoil, 1)[0];
    if (run != prevRun)
        Log(run
            ? $"SCADA a écrit M{StartCoil} = 1 (bouton DÉMARRER)   -> la machine est autorisée, les boutons moteurs deviennent actifs"
            : $"SCADA a écrit M{StartCoil} = 0 (bouton ARRÊTER)    -> l'automate va couper tous les moteurs");

    for (var n = 0; n < Motors; n++)
    {
        var fwd = coils.ReadPoints((ushort)(2 * n), 1)[0];
        var bwd = coils.ReadPoints((ushort)(2 * n + 1), 1)[0];
        var sp = regs.ReadPoints((ushort)(SetpointStart + n), 1)[0];

        if (fwd != prevFwd[n])
            Log($"SCADA a écrit M{2 * n} = {(fwd ? 1 : 0)} ({MotorWord[n]}, " + (fwd ? "bouton Avant)" : "avant coupé)"));
        if (bwd != prevBwd[n])
            Log($"SCADA a écrit M{2 * n + 1} = {(bwd ? 1 : 0)} ({MotorWord[n]}, " + (bwd ? "bouton Arrière)" : "arrière coupé)"));
        if (sp != prevSet[n])
        {
            Log($"SCADA a écrit D{SetpointStart + n} = {sp} (consigne du {MotorWord[n]}, bouton Appliquer)");
            reached[n] = false;
        }

        // 2) Logique « Ladder » : sécurités.
        if (!run || (fwd && bwd))
        {
            if (fwd || bwd)
                Log(!run
                    ? $"Ladder : M{StartCoil} = 0 -> remet à 0 les relais du {MotorWord[n]} (M{2 * n}/M{2 * n + 1})"
                    : $"Ladder : M{2 * n} et M{2 * n + 1} à 1 en même temps (interdit) -> les deux sont remis à 0");
            if (fwd) coils.WritePoints((ushort)(2 * n), [false]);
            if (bwd) coils.WritePoints((ushort)(2 * n + 1), [false]);
            fwd = bwd = false;
        }

        // 3) Sortie : la vitesse suit la cible avec de l'inertie.
        var running = fwd || bwd;
        double target = running ? sp : 0;
        speed[n] += (target - speed[n]) * Gain;
        var shown = (ushort)Math.Round(speed[n]);
        regs.WritePoints((ushort)(FeedbackStart + n), [shown]);

        if (running && !wasRunning[n])
        {
            Log($"Ladder : {MotorWord[n]} démarre -> D{FeedbackStart + n} monte de {shown} vers la cible {target} (consigne D{SetpointStart + n})");
            reached[n] = false;
        }
        else if (!running && wasRunning[n])
        {
            Log($"Ladder : {MotorWord[n]} arrêté -> la cible passe à 0, D{FeedbackStart + n} redescend");
            reached[n] = false;
        }

        if (running && !reached[n] && Math.Abs(speed[n] - target) < 0.5)
        {
            reached[n] = true;
            Log($"Ladder : {MotorWord[n]} a atteint sa vitesse (D{FeedbackStart + n} = {shown} = consigne)");
        }

        wasRunning[n] = running;
        prevFwd[n] = fwd;
        prevBwd[n] = bwd;
        prevSet[n] = sp;
    }
    prevRun = run;

    // 4) Affichage de l'état courant.
    WriteAt(liveRow, $" M{StartCoil} marche machine : {(run ? "1  EN MARCHE" : "0  ARRÊTÉE")}");
    for (var n = 0; n < Motors; n++)
    {
        var f = coils.ReadPoints((ushort)(2 * n), 1)[0];
        var b = coils.ReadPoints((ushort)(2 * n + 1), 1)[0];
        var sens = f ? "AVANT   " : b ? "ARRIÈRE " : "ARRÊTÉ  ";
        var sp = regs.ReadPoints((ushort)(SetpointStart + n), 1)[0];
        var fb = regs.ReadPoints((ushort)(FeedbackStart + n), 1)[0];
        var cible = (f || b) ? sp : 0;
        WriteAt(liveRow + 1 + n,
            $" Moteur {n + 1}  M{2 * n}={(f ? 1 : 0)} M{2 * n + 1}={(b ? 1 : 0)} {sens} " +
            $"consigne D{SetpointStart + n}={sp,5}  cible={cible,5}  mesure D{FeedbackStart + n}={fb,5}  {Bar(fb, sp)}");
    }
    WriteAt(liveRow + 1 + Motors, new string('=', 100));
    WriteAt(liveRow + 2 + Motors, "JOURNAL (le plus récent en bas)");
    for (var i = 0; i < LogLines; i++)
        WriteAt(logRow + 1 + i, i < log.Count ? " " + log[i] : "");

    try { await Task.Delay(ScanMs, cts.Token); } catch (OperationCanceledException) { }
}

network.Dispose();
listener.Stop();
