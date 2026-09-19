namespace BrewInventory.Application.Contracts.Fermentables;

public record FermentableResponse(
    int Id,
    string Name,
    double Amount,
    string? BrewfatherId,
    string? Supplier,
    string? Origin,
    string? Type,
    double Color,
    string? GrainCategory,
    int? Percentage,
    double Lovibond);
