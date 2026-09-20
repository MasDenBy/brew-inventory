using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Recipes;

public record CreateRecipeYeastRequest(
    string Name,
    double Amount,
    string? Laboratory,
    YeastType Type,
    YeastForm Form,
    double? Attenuation,
    string? Unit);
