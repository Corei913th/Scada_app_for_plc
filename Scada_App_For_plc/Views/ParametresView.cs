using Scada_App_For_plc.Services;

namespace Scada_App_For_plc.Views;

/// <summary>Consignes de vitesse (écriture automate) et mesures (lecture), avec archivage en base.</summary>
public partial class ParametresView : ViewBase
{
    private readonly NumericUpDown[] _setpoints;   // consignes saisies → D3000..D3002
    private readonly Label[] _feedback;            // mesures affichées ← D3100..D3102
    private int[]? _lastFeedback;                  // dernière mesure lue, celle qu'on archive en base
    private bool _setpointsLoaded;                 // les consignes de l'automate ont-elles déjà été relues ?
    private bool _busy;

    public ParametresView()
    {
        InitializeComponent();
        MessageLabel = lblMessage;

        _setpoints = [numSetpoint1, numSetpoint2, numSetpoint3];
        _feedback = [lblFeedback1, lblFeedback2, lblFeedback3];
    }

    // Lecture des mesures seulement quand la page est visible.
    private void ParametresView_VisibleChanged(object? sender, EventArgs e)
    {
        if (IsReady) tmrFeedback.Enabled = Visible;
    }

    /// <summary>Toutes les 500 ms : lit les vitesses mesurées (D3100..D3102) et les affiche.</summary>
    private async void TmrFeedback_Tick(object? sender, EventArgs e)
    {
        if (_busy || !IsReady || !Svc.Plc.IsConnected) return;
        _busy = true;
        try
        {
            // À la première connexion on préremplit les consignes avec celles de l'automate.
            if (!_setpointsLoaded)
                await LoadSetpointsAsync();

            _lastFeedback = await Svc.Plc.ReadFeedbackAsync();
            for (var i = 0; i < _feedback.Length; i++)
                _feedback[i].Text = _lastFeedback[i].ToString();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
        finally
        {
            _busy = false;
        }
    }

    private async void BtnApply_Click(object? sender, EventArgs e) => await SafeAsync(ApplyAsync);

    private async void BtnReload_Click(object? sender, EventArgs e) => await SafeAsync(LoadSetpointsAsync);

    private async void BtnSave_Click(object? sender, EventArgs e) => await SafeAsync(SaveAsync);

    /// <summary>Relit les consignes D3000..D3002 dans l'automate et préremplit les champs de saisie.</summary>
    private async Task LoadSetpointsAsync()
    {
        var values = await Svc.Plc.ReadSetpointsAsync();
        for (var i = 0; i < _setpoints.Length; i++)
            _setpoints[i].Value = Math.Clamp(values[i], 0, ushort.MaxValue);
        _setpointsLoaded = true;
        ShowInfo("Consignes relues depuis l'automate.");
    }

    /// <summary>Envoie les 3 consignes saisies vers D3000..D3002 ; le Ladder les utilise au cycle suivant.</summary>
    private async Task ApplyAsync()
    {
        var values = _setpoints.Select(n => (int)n.Value).ToArray();
        await Svc.Plc.WriteSetpointsAsync(values);
        ShowInfo("Consignes envoyées à l'automate.");
    }

    /// <summary>Archive dans PostgreSQL la dernière mesure lue (D3100..D3102).</summary>
    private async Task SaveAsync()
    {
        if (_lastFeedback is null)
        {
            ShowError("Aucune mesure disponible : l'automate n'est pas connecté.");
            return;
        }

        await Svc.Repo.InsertAsync(_lastFeedback);
        ShowInfo("Mesures enregistrées dans PostgreSQL.");
    }

    private void lblFeedback1_Click(object sender, EventArgs e)
    {

    }
}
