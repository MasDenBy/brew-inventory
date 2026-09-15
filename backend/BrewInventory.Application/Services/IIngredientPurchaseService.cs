using BrewInventory.Application.Contracts.IngredientPurchase;

namespace BrewInventory.Application.Services;

public interface IIngredientPurchaseService
{
    Task<IngredientPurchaseResponse> CalculateIngredientNeedsAsync(ICollection<int> recipeIds, CancellationToken cancellationToken = default);
    Task<byte[]> ExportToExcelAsync(ICollection<int> recipeIds, CancellationToken cancellationToken = default);
}
