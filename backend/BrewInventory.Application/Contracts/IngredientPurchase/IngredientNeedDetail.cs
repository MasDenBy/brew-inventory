namespace BrewInventory.Application.Contracts.IngredientPurchase;

public record IngredientNeedDetail(
    int IngredientId,
    string Name,
    string Type,
    decimal AmountNeeded,
    decimal AmountInInventory,
    decimal AmountToBuy,
    string? Unit);
