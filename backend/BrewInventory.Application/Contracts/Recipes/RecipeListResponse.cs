namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeListResponse(
    int Id,
    string Name,
    string? BrewfatherId);
