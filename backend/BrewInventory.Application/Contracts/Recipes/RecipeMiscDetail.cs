namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeMiscDetail(
    string Name,
    double Amount,
    string Type,
    string? Unit,
    string? Use,
    double? Time);
