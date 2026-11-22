using Scada_App_For_plc.Services;

namespace Scada_App_For_plc.Views;

/// <summary>
/// Mesures (lecture) et archivage en base. Les consignes de vitesse se règlent directement
/// depuis l'écran Principal, sur la carte de chaque moteur.
/// </summary>
public partial class ParametresView : ViewBase
{
    private readonly Label[] _feedback;            // mesures affichées ← D3100..D3102
    private int[]? _lastFeedback;                  // dernière mesure lue, celle qu'on archive en base
    private bool _busy;

    public ParametresView()
    {
        InitializeComponent();
        MessageLabel = lblMessage;

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

    private async void BtnSave_Click(object? sender, EventArgs e) => await SafeAsync(SaveAsync);

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
}
