namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherCreateRecipeRequest(
    string name,
    string type,
    ICollection<BrewfatherRecipeFermentable> fermentables,
    ICollection<BrewfatherRecipeHop> hops,
    ICollection<BrewfatherRecipeMisc> miscs,
    ICollection<BrewfatherRecipeYeast> yeasts
);
