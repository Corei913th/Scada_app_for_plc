using Scada_App_For_plc.Services;

namespace Scada_App_For_plc.Views;

/// <summary>
/// Base des vues. Constructeur sans paramètre (requis par le concepteur Visual Studio) :
/// les services sont fournis après coup par <see cref="Initialize"/>.
/// </summary>
public class ViewBase : UserControl
{
    private AppServices? _svc;

    protected AppServices Svc => _svc ?? throw new InvalidOperationException("Vue non initialisée.");

    /// <summary>Vrai une fois <see cref="Initialize"/> appelée (faux dans le concepteur).</summary>
    protected bool IsReady => _svc is not null;

    /// <summary>Barre de message de la vue ; chaque vue y affecte son label dans son constructeur.</summary>
    protected Label? MessageLabel { get; set; }

    public void Initialize(AppServices svc)
    {
        _svc = svc;
        OnInitialized();
    }

    protected virtual void OnInitialized()
    {
    }

    protected void ShowInfo(string text) => SetMessage(text, Color.DarkGreen);

    protected void ShowError(string text) => SetMessage(text, Color.Firebrick);

    protected void ClearMessage()
    {
        if (MessageLabel is not null)
            MessageLabel.Text = "";
    }

    private void SetMessage(string text, Color color)
    {
        if (MessageLabel is null)
            return;
        MessageLabel.ForeColor = color;
        MessageLabel.Text = text;
    }

    /// <summary>Exécute une action en affichant toute exception dans la barre de message.</summary>
    protected async Task SafeAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }
}
