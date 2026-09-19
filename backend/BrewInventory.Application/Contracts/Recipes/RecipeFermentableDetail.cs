namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeFermentableDetail(
    int FermentableId,
    string Name,
    string Type,
    double Amount,
    string? Supplier,
    string? Origin,
    double Color);
