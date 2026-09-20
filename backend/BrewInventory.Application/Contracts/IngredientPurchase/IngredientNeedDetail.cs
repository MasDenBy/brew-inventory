namespace BrewInventory.Application.Contracts.IngredientPurchase;

public record IngredientNeedDetail(
    string Name,
    string Type,
    double AmountNeeded,
    double AmountInInventory,
    double AmountToBuy,
    string? Unit,
    string? Supplier = null,
    string? ProductId = null);
