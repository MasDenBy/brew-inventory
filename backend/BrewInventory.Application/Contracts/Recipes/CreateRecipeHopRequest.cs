using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Recipes;

public record CreateRecipeHopRequest(
    string Name,
    double Amount,
    double? Alpha,
    HopType Type,
    string? Origin,
    HopUse Use,
    double? Time);
