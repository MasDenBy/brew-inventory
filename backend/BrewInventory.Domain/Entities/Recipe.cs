namespace BrewInventory.Domain.Entities;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? BrewfatherId { get; set; }
    public string? Style { get; set; }
    public string? Notes { get; set; }

    public ICollection<RecipeFermentable> RecipeFermentables { get; } = [];
    public ICollection<RecipeHop> RecipeHops { get; } = [];
    public ICollection<RecipeYeast> RecipeYeasts { get; } = [];
    public ICollection<RecipeMisc> RecipeMiscs { get; } = [];

    public void AddFermentables(IEnumerable<RecipeFermentable> fermentables)
    {
        ArgumentNullException.ThrowIfNull(fermentables);

        foreach (var fermentable in fermentables)
        {
            RecipeFermentables.Add(fermentable);
        }
    }

    public void AddHops(IEnumerable<RecipeHop> hops)
    {
        ArgumentNullException.ThrowIfNull(hops);

        foreach (var hop in hops)
        {
            RecipeHops.Add(hop);
        }
    }

    public void AddYeasts(IEnumerable<RecipeYeast> yeasts)
    {
        ArgumentNullException.ThrowIfNull(yeasts);

        foreach (var yeast in yeasts)
        {
            RecipeYeasts.Add(yeast);
        }
    }

    public void AddMiscs(IEnumerable<RecipeMisc> miscs)
    {
        ArgumentNullException.ThrowIfNull(miscs);

        foreach (var misc in miscs)
        {
            RecipeMiscs.Add(misc);
        }
    }
}
