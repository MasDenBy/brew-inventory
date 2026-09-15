namespace BrewInventory.Application.Contracts.IngredientPurchase;

public record IngredientPurchaseResponse(
    ICollection<IngredientNeedDetail> Fermentables,
    ICollection<IngredientNeedDetail> Hops,
    ICollection<IngredientNeedDetail> Yeasts,
    ICollection<IngredientNeedDetail> Miscs);
