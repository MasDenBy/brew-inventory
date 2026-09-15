namespace BrewInventory.Application.Contracts.Recipes;

public record RecipeDetailsResponse(
    int Id,
    string Name,
    string? BrewfatherId,
    ICollection<RecipeFermentableDetail> Fermentables,
    ICollection<RecipeHopDetail> Hops,
    ICollection<RecipeYeastDetail> Yeasts,
    ICollection<RecipeMiscDetail> Miscs);
