namespace BrewInventory.Application.Contracts.IngredientPurchase;

public record IngredientNeedDetail(
    int IngredientId,
    string Name,
    string Type,
    double AmountNeeded,
    double AmountInInventory,
    double AmountToBuy,
    string? Unit);
