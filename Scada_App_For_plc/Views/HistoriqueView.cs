using Scada_App_For_plc.Services;

namespace Scada_App_For_plc.Views;

/// <summary>Historique des mesures archivées (PostgreSQL) et export Excel.</summary>
public partial class HistoriqueView : ViewBase
{
    private List<Mesure> _rows = [];

    public HistoriqueView()
    {
        InitializeComponent();
        MessageLabel = lblMessage;
    }

    private async void HistoriqueView_VisibleChanged(object? sender, EventArgs e)
    {
        if (Visible && IsReady)
            await SafeAsync(RefreshAsync);
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e) => await SafeAsync(RefreshAsync);

    private async void BtnExport_Click(object? sender, EventArgs e) => await SafeAsync(ExportAsync);

    /// <summary>Recharge les mesures archivées (les plus récentes en premier) dans la grille.</summary>
    private async Task RefreshAsync()
    {
        _rows = await Svc.Repo.GetLatestAsync();
        dataGrid.DataSource = _rows;
        if (dataGrid.Columns["Horodatage"] is { } col)
            col.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
        ShowInfo($"{_rows.Count} mesure(s) chargée(s).");
    }

    /// <summary>Demande un fichier .xlsx à l'utilisateur puis y exporte les lignes affichées.</summary>
    private async Task ExportAsync()
    {
        if (_rows.Count == 0)
        {
            ShowError("Rien à exporter : l'historique est vide.");
            return;
        }

        using var dialog = new SaveFileDialog
        {
            Filter = "Classeur Excel (*.xlsx)|*.xlsx",
            FileName = $"mesures_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        var rows = _rows.ToList();
        await Task.Run(() => ExcelExporter.Export(dialog.FileName, rows));
        ShowInfo("Export terminé : " + dialog.FileName);
    }
}
