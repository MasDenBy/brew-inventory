namespace BrewInventory.Application.Contracts.Recipes;

public record SyncRecipeResponse(
    int Id,
    string Name,
    string BrewfatherId);
