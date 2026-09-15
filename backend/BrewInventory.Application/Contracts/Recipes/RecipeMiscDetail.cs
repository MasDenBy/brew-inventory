namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeMiscDetail(
    int MiscId,
    string Name,
    string Type,
    string Unit,
    decimal Amount);
