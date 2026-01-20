using BrewInventory.App.Data;
using BrewInventory.App.Data.Entities;
using BrewInventory.App.Models.Contracts;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Endpoints;

public static class IngredientPurchaseEndpoints
{
    public static void MapIngredientPurchaseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/ingredient-purchase");

        group.MapPost("/calculate", async (IngredientPurchaseRequest request, BrewInventoryContext db) =>
        {
            var validationResult = ValidateRequest(request);
            if (validationResult != null)
            {
                return validationResult;
            }

            var recipes = await LoadRecipesAsync(request.RecipeIds, db);
            if (recipes == null || recipes.Count == 0)
            {
                return Results.NotFound("No recipes found with the provided IDs.");
            }

            var (fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds) = CalculateIngredientNeeds(recipes);

            var response = new IngredientPurchaseResponse(
                fermentableNeeds,
                hopNeeds,
                yeastNeeds,
                miscNeeds);

            return Results.Ok(response);
        });

        group.MapPost("/export", async (IngredientPurchaseRequest request, BrewInventoryContext db) =>
        {
            var validationResult = ValidateRequest(request);
            if (validationResult != null)
            {
                return validationResult;
            }

            var recipes = await LoadRecipesAsync(request.RecipeIds, db);
            if (recipes == null || recipes.Count == 0)
            {
                return Results.NotFound("No recipes found with the provided IDs.");
            }

            var (fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds) = CalculateIngredientNeeds(recipes);

            var excelBytes = GenerateExcel(fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds);
            var fileName = $"ingredient-purchase-{DateTime.UtcNow:yyyyMMdd-HHmmss}.xlsx";

            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        });
    }

    private static IResult? ValidateRequest(IngredientPurchaseRequest request)
    {
        if (request.RecipeIds == null || request.RecipeIds.Count == 0)
        {
            return Results.BadRequest("At least one recipe ID is required.");
        }

        return null;
    }

    private static async Task<List<Recipe>> LoadRecipesAsync(List<int> recipeIds, BrewInventoryContext db)
    {
        return await db.Recipes
            .Include(r => r.RecipeFermentables)
                .ThenInclude(rf => rf.Fermentable)
            .Include(r => r.RecipeHops)
                .ThenInclude(rh => rh.Hop)
            .Include(r => r.RecipeYeasts)
                .ThenInclude(ry => ry.Yeast)
            .Include(r => r.RecipeMiscs)
                .ThenInclude(rm => rm.Misc)
            .Where(r => recipeIds.Contains(r.Id))
            .ToListAsync();
    }

    private static (List<IngredientNeedDetail> fermentables, List<IngredientNeedDetail> hops, List<IngredientNeedDetail> yeasts, List<IngredientNeedDetail> miscs) CalculateIngredientNeeds(List<Recipe> recipes)
    {
        var fermentableNeeds = CalculateFermentableNeeds(recipes);
        var hopNeeds = CalculateHopNeeds(recipes);
        var yeastNeeds = CalculateYeastNeeds(recipes);
        var miscNeeds = CalculateMiscNeeds(recipes);

        return (fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds);
    }

    private static List<IngredientNeedDetail> CalculateFermentableNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeFermentables)
            .GroupBy(rf => rf.FermentableId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Fermentable = g.First().Fermentable,
                TotalNeeded = g.Sum(rf => rf.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Fermentable.Name,
                item.Fermentable.Type.ToString(),
                item.TotalNeeded,
                item.Fermentable.Amount,
                Math.Max(0, item.TotalNeeded - item.Fermentable.Amount),
                "kg"))
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateHopNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeHops)
            .GroupBy(rh => rh.HopId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Hop = g.First().Hop,
                TotalNeeded = g.Sum(rh => rh.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Hop.Name,
                item.Hop.Type.ToString(),
                item.TotalNeeded,
                item.Hop.Amount,
                Math.Max(0, item.TotalNeeded - item.Hop.Amount),
                "g"))
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateYeastNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeYeasts)
            .GroupBy(ry => ry.YeastId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Yeast = g.First().Yeast,
                TotalNeeded = g.Sum(ry => ry.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Yeast.Name,
                item.Yeast.Type.ToString(),
                item.TotalNeeded,
                item.Yeast.Amount,
                Math.Max(0, item.TotalNeeded - item.Yeast.Amount),
                item.Yeast.Form.ToString()))
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateMiscNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeMiscs)
            .GroupBy(rm => rm.MiscId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Misc = g.First().Misc,
                TotalNeeded = g.Sum(rm => rm.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Misc.Name,
                item.Misc.Type.ToString(),
                item.TotalNeeded,
                item.Misc.Amount,
                Math.Max(0, item.TotalNeeded - item.Misc.Amount),
                item.Misc.Unit.ToString()))
            .ToList();
    }

    private static byte[] GenerateExcel(
        List<IngredientNeedDetail> fermentables,
        List<IngredientNeedDetail> hops,
        List<IngredientNeedDetail> yeasts,
        List<IngredientNeedDetail> miscs)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Ingredient Purchase List");

        // Add header
        worksheet.Cell(1, 1).Value = "Category";
        worksheet.Cell(1, 2).Value = "Ingredient ID";
        worksheet.Cell(1, 3).Value = "Name";
        worksheet.Cell(1, 4).Value = "Type";
        worksheet.Cell(1, 5).Value = "Amount Needed";
        worksheet.Cell(1, 6).Value = "Amount In Inventory";
        worksheet.Cell(1, 7).Value = "Amount To Buy";
        worksheet.Cell(1, 8).Value = "Unit";

        // Style header
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        int currentRow = 2;

        // Add fermentables
        foreach (var item in fermentables)
        {
            AddIngredientRow(worksheet, currentRow++, "Fermentable", item);
        }

        // Add hops
        foreach (var item in hops)
        {
            AddIngredientRow(worksheet, currentRow++, "Hop", item);
        }

        // Add yeasts
        foreach (var item in yeasts)
        {
            AddIngredientRow(worksheet, currentRow++, "Yeast", item);
        }

        // Add miscs
        foreach (var item in miscs)
        {
            AddIngredientRow(worksheet, currentRow++, "Misc", item);
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        // Highlight items that need to be purchased
        for (int row = 2; row < currentRow; row++)
        {
            var amountToBuy = worksheet.Cell(row, 7).Value;
            if (amountToBuy.IsNumber && amountToBuy.GetNumber() > 0)
            {
                worksheet.Range(row, 1, row, 8).Style.Fill.BackgroundColor = XLColor.LightYellow;
            }
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void AddIngredientRow(IXLWorksheet worksheet, int row, string category, IngredientNeedDetail item)
    {
        worksheet.Cell(row, 1).Value = category;
        worksheet.Cell(row, 2).Value = item.IngredientId;
        worksheet.Cell(row, 3).Value = item.Name;
        worksheet.Cell(row, 4).Value = item.Type;
        worksheet.Cell(row, 5).Value = item.AmountNeeded;
        worksheet.Cell(row, 6).Value = item.AmountInInventory;
        worksheet.Cell(row, 7).Value = item.AmountToBuy;
        worksheet.Cell(row, 8).Value = item.Unit;

        // Format numeric columns
        worksheet.Cell(row, 5).Style.NumberFormat.Format = "0.00";
        worksheet.Cell(row, 6).Style.NumberFormat.Format = "0.00";
        worksheet.Cell(row, 7).Style.NumberFormat.Format = "0.00";
    }
}
