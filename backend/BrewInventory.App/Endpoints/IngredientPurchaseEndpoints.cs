using System.Globalization;
using BrewInventory.Application.Contracts.IngredientPurchase;
using BrewInventory.Application.Services;

namespace BrewInventory.App.Endpoints;

internal static class IngredientPurchaseEndpoints
{
    public static void MapIngredientPurchaseEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/ingredient-purchase");

        group.MapPost("/calculate", async (IngredientPurchaseRequest request, IIngredientPurchaseService service, CancellationToken ct = default) =>
        {
            if (request.RecipeIds == null || request.RecipeIds.Count == 0)
            {
                return Results.BadRequest("At least one recipe ID is required.");
            }

            var response = await service.CalculateIngredientNeedsAsync(request.RecipeIds, ct);
            return Results.Ok(response);
        });

        group.MapPost("/export", async (IngredientPurchaseRequest request, IIngredientPurchaseService service) =>
        {
            if (request.RecipeIds == null || request.RecipeIds.Count == 0)
            {
                return Results.BadRequest("At least one recipe ID is required.");
            }

            var excelBytes = await service.ExportToExcelAsync(request.RecipeIds);
            if (excelBytes.Length == 0)
            {
                return Results.NotFound("No recipes found with the provided IDs.");
            }

            var fileName = $"ingredient-purchase-{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture)}.xlsx";
            return Results.File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        });
    }
}
