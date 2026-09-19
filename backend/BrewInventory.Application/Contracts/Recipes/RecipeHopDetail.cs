namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeHopDetail(
    int HopId,
    string Name,
    string Type,
    double Amount,
    string? Origin,
    double AlphaAcid,
    int? HarvestYear);
