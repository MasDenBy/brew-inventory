using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Recipes;

public record CreateRecipeFermentableRequest(
    string Name,
    double Amount,
    FermentableType Type,
    string? Supplier,
    string? Origin,
    double? Color,
    double? Potential);
