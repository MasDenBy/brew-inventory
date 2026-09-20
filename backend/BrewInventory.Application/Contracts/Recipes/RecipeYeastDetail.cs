namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeYeastDetail(
    string Name,
    double Amount,
    string? Laboratory,
    string Type,
    string Form,
    double? Attenuation,
    string? Unit);
