using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Recipes;

public record CreateRecipeMiscRequest(
    string Name,
    double Amount,
    MiscType Type,
    string? Unit,
    string? Use,
    double? Time);
