namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeYeastDetail(
    int YeastId,
    string Name,
    string Type,
    string Form,
    decimal Amount,
    string Laboratory);
