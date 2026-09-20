namespace BrewInventory.Application.Contracts.Fermentables;

public record CreateFermentableRequest(
    string Name,
    double Amount,
    string? BrewfatherId,
    string? Supplier,
    string? Origin,
    string? Type,
    double Color,
    string? GrainCategory,
    double? Percentage,
    double Lovibond);
