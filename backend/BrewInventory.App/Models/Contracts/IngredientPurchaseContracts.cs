namespace BrewInventory.App.Models.Contracts;

public record IngredientPurchaseRequest(List<int> RecipeIds);

public record IngredientPurchaseResponse(
    List<IngredientNeedDetail> Fermentables,
    List<IngredientNeedDetail> Hops,
    List<IngredientNeedDetail> Yeasts,
    List<IngredientNeedDetail> Miscs);

public record IngredientNeedDetail(
    int IngredientId,
    string Name,
    string Type,
    decimal AmountNeeded,
    decimal AmountInInventory,
    decimal AmountToBuy,
    string? Unit);