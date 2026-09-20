using BrewInventory.Application.Contracts.IngredientPurchase;
using BrewInventory.Application.Services;
using ClosedXML.Excel;

namespace BrewInventory.Infrastructure.Services;

public class ExcelExporter : IExcelExporter
{
    public byte[] GeneratePurchaseList(
        ICollection<IngredientNeedDetail> fermentables,
        ICollection<IngredientNeedDetail> hops,
        ICollection<IngredientNeedDetail> yeasts,
        ICollection<IngredientNeedDetail> miscs)
    {
        ArgumentNullException.ThrowIfNull(fermentables);
        ArgumentNullException.ThrowIfNull(hops);
        ArgumentNullException.ThrowIfNull(yeasts);
        ArgumentNullException.ThrowIfNull(miscs);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Ingredient Purchase List");

        worksheet.Cell(1, 1).Value = "Category";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Type";
        worksheet.Cell(1, 4).Value = "Amount Needed";
        worksheet.Cell(1, 5).Value = "Amount In Inventory";
        worksheet.Cell(1, 6).Value = "Amount To Buy";
        worksheet.Cell(1, 7).Value = "Unit";

        var headerRange = worksheet.Range(1, 1, 1, 7);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        int currentRow = 2;

        foreach (var item in fermentables)
        {
            AddIngredientRow(worksheet, currentRow++, "Fermentable", item);
        }

        foreach (var item in hops)
        {
            AddIngredientRow(worksheet, currentRow++, "Hop", item);
        }

        foreach (var item in yeasts)
        {
            AddIngredientRow(worksheet, currentRow++, "Yeast", item);
        }

        foreach (var item in miscs)
        {
            AddIngredientRow(worksheet, currentRow++, "Misc", item);
        }

        worksheet.Columns().AdjustToContents();

        for (int row = 2; row < currentRow; row++)
        {
            var amountToBuy = worksheet.Cell(row, 6).Value;
            if (amountToBuy.IsNumber && amountToBuy.GetNumber() > 0)
            {
                worksheet.Range(row, 1, row, 7).Style.Fill.BackgroundColor = XLColor.LightYellow;
            }
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void AddIngredientRow(IXLWorksheet worksheet, int row, string category, IngredientNeedDetail item)
    {
        worksheet.Cell(row, 1).Value = category;
        worksheet.Cell(row, 2).Value = item.Name;
        worksheet.Cell(row, 3).Value = item.Type;
        worksheet.Cell(row, 4).Value = item.AmountNeeded;
        worksheet.Cell(row, 5).Value = item.AmountInInventory;
        worksheet.Cell(row, 6).Value = item.AmountToBuy;
        worksheet.Cell(row, 7).Value = item.Unit;

        worksheet.Cell(row, 4).Style.NumberFormat.Format = "0.00";
        worksheet.Cell(row, 5).Style.NumberFormat.Format = "0.00";
        worksheet.Cell(row, 6).Style.NumberFormat.Format = "0.00";
    }
}
