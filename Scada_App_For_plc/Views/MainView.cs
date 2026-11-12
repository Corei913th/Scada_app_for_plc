using Scada_App_For_plc.Services;

namespace Scada_App_For_plc.Views;

/// <summary>Page principale : marche/arrêt machine et pilotage des 3 moteurs.</summary>
public partial class MainView : ViewBase
{
    private readonly MotorPanel[] _motors;
    private bool _busy;

    public MainView()
    {
        InitializeComponent();
        MessageLabel = lblMessage;

        _motors = [motorPanel1, motorPanel2, motorPanel3];
        for (var i = 0; i < _motors.Length; i++)
        {
            var index = i; // capture pour la lambda
            // Clic sur Avant / Arrière / Arrêt d'une carte moteur → on écrit ses relais M dans l'automate.
            // (moteur 0 = M0/M1, moteur 1 = M2/M3, moteur 2 = M4/M5)
            _motors[i].DirectionRequested += dir =>
            {
                if (IsReady) _ = SafeAsync(() => Svc.Plc.SetMotorAsync(index, dir));
            };
        }
    }

    protected override void OnInitialized()
    {
        // Période de lecture cyclique de l'automate (PollMs), jamais sous 50 ms pour ne pas saturer la liaison.
        tmrPoll.Interval = Math.Max(50, Svc.Settings.Plc.PollMs);
    }

    // Bouton « Démarrer » : met M20 (StartCoil) à 1 ; le Ladder autorise alors les moteurs.
    private void BtnStart_Click(object? sender, EventArgs e) =>
        _ = SafeAsync(() => Svc.Plc.SetMachineRunningAsync(true));

    // Bouton « Arrêt général » : remet M20 à 0.
    private void BtnStopAll_Click(object? sender, EventArgs e) =>
        _ = SafeAsync(() => Svc.Plc.SetMachineRunningAsync(false));

    // On ne lit l'automate que lorsque la page est affichée (le timer s'arrête sinon).
    private void MainView_VisibleChanged(object? sender, EventArgs e)
    {
        if (!IsReady) return;
        tmrPoll.Enabled = Visible;
        if (Visible) ShowOffline();
    }

    /// <summary>
    /// Cycle de supervision : lit les relais M0..M20 puis met à jour le texte « Machine » et les 3 cartes moteur.
    /// Le drapeau _busy évite d'empiler les lectures si l'automate répond plus lentement que le timer.
    /// </summary>
    private async void TmrPoll_Tick(object? sender, EventArgs e)
    {
        if (_busy || !IsReady) return;
        if (!Svc.Plc.IsConnected)
        {
            ShowOffline();
            return;
        }

        _busy = true;
        try
        {
            var status = await Svc.Plc.ReadStatusAsync();
            // Vitesses réelles (D3100..D3102) : elles font tourner les hélices.
            var speeds = await Svc.Plc.ReadFeedbackAsync();
            lblMachine.Text = status.MachineRunning ? "Machine : EN MARCHE" : "Machine : ARRÊTÉE";
            lblMachine.ForeColor = status.MachineRunning ? Color.DarkGreen : Color.Firebrick;
            // Les boutons de chaque moteur ne sont actifs que si M20 (marche machine) est à 1.
            for (var i = 0; i < _motors.Length; i++)
                _motors[i].SetState(status.Motors[i], status.MachineRunning, speeds[i]);
            ClearMessage();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
            ShowOffline();
        }
        finally
        {
            _busy = false;
        }
    }

    /// <summary>État affiché quand l'automate est injoignable : voyants gris et boutons grisés.</summary>
    private void ShowOffline()
    {
        lblMachine.Text = "Machine : automate non connecté";
        lblMachine.ForeColor = Color.Gray;
        foreach (var m in _motors) m.SetUnknown();
    }
}
