namespace BrewInventory.Application.Contracts.Recipes;

public record CreateRecipeRequest(
    string Name,
    string? Style,
    ICollection<CreateRecipeFermentableRequest> Fermentables,
    ICollection<CreateRecipeHopRequest> Hops,
    ICollection<CreateRecipeYeastRequest> Yeasts,
    ICollection<CreateRecipeMiscRequest> Miscs);
