namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipe(
    string _id,
    string name,
    string? author,
    string? type,
    BrewfatherRecipeEquipment? equipment,
    BrewfatherRecipeStyle? style,
    ICollection<BrewfatherRecipeFermentable>? fermentables,
    ICollection<BrewfatherRecipeHop>? hops,
    ICollection<BrewfatherRecipeMisc>? miscs,
    ICollection<BrewfatherRecipeYeast>? yeasts
);
