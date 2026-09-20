using System.Text.Json.Serialization;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipe
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public ICollection<BrewfatherRecipeFermentable>? Fermentables { get; init; }
    public ICollection<BrewfatherRecipeHop>? Hops { get; init; }
    public ICollection<BrewfatherRecipeMisc>? Miscs { get; init; }
    public BrewfatherRecipeStyle? Style { get; init; }
    public ICollection<BrewfatherRecipeYeast>? Yeasts { get; init; }
}
