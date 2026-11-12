using Npgsql;

namespace Scada_App_For_plc.Views;

/// <summary>Page d'information : configuration active (sans le mot de passe de la base).</summary>
public partial class InfoView : ViewBase
{
    public InfoView()
    {
        InitializeComponent();
        MessageLabel = lblMessage;
    }

    protected override void OnInitialized()
    {
        var p = Svc.Settings.Plc;
        var db = new NpgsqlConnectionStringBuilder(Svc.Settings.Database.ConnectionString);

        lblInfo.Text =
            "SCADA pour automate Delta — projet de cours EADL-1\n" +
            "Supervision et pilotage par Modbus TCP, archivage PostgreSQL, export Excel.\n\n" +
            $"Automate : {p.Ip}:{p.Port} (unité {p.UnitId})\n" +
            // Numéros de relais M tels qu'écrits dans le Ladder (l'offset Modbus n'intervient pas ici).
            "Commandes moteurs : M0..M5   (avant = M0/M2/M4, arrière = M1/M3/M5)\n" +
            $"Marche machine : M{p.StartCoil}\n" +
            $"Consignes : D{p.SetpointStart}..D{p.SetpointStart + 2}   Mesures : D{p.FeedbackStart}..D{p.FeedbackStart + 2}\n\n" +
            $"Base : {db.Host}:{db.Port} / {db.Database} (utilisateur {db.Username})\n\n" +
            "Les adresses se règlent dans appsettings.json.";
    }
}
