namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeHopDetail(
    string Name,
    double Amount,
    double? Alpha,
    string Type,
    string? Origin,
    string Use,
    double? Time);
