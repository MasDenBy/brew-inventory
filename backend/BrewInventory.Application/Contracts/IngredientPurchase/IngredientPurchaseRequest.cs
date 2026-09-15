namespace BrewInventory.Application.Contracts.IngredientPurchase;

public record IngredientPurchaseRequest(ICollection<int> RecipeIds);
