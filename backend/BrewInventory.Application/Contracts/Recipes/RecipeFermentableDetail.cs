namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeFermentableDetail(
    string Name,
    double Amount,
    string Type,
    string? Supplier,
    string? Origin,
    double? Color,
    double? Potential);
