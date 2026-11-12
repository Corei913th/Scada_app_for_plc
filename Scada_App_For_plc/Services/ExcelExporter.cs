using ClosedXML.Excel;

namespace Scada_App_For_plc.Services;

/// <summary>Export des mesures vers un classeur .xlsx (Excel n'a pas besoin d'être installé).</summary>
public static class ExcelExporter
{
    /// <summary>Écrit une feuille « Mesures » : une ligne d'en-tête, puis une ligne par mesure (Id, date, 3 vitesses).</summary>
    public static void Export(string path, IReadOnlyList<Mesure> mesures)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Mesures");

        string[] headers = ["Id", "Date / heure", "Moteur 1", "Moteur 2", "Moteur 3"];
        for (var c = 0; c < headers.Length; c++)
            ws.Cell(1, c + 1).Value = headers[c];
        ws.Row(1).Style.Font.Bold = true;

        for (var r = 0; r < mesures.Count; r++)
        {
            var m = mesures[r];
            var row = r + 2;
            ws.Cell(row, 1).Value = m.Id;
            ws.Cell(row, 2).Value = m.Horodatage;
            ws.Cell(row, 2).Style.DateFormat.Format = "dd/MM/yyyy HH:mm:ss";
            ws.Cell(row, 3).Value = m.Moteur1;
            ws.Cell(row, 4).Value = m.Moteur2;
            ws.Cell(row, 5).Value = m.Moteur3;
        }

        ws.Columns().AdjustToContents();
        wb.SaveAs(path);
    }
}
