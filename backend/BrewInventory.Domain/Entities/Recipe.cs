namespace BrewInventory.Domain.Entities;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? BrewfatherId { get; set; }

    public ICollection<RecipeFermentable> RecipeFermentables { get; } = new List<RecipeFermentable>();
    public ICollection<RecipeHop> RecipeHops { get; } = new List<RecipeHop>();
    public ICollection<RecipeYeast> RecipeYeasts { get; } = new List<RecipeYeast>();
    public ICollection<RecipeMisc> RecipeMiscs { get; } = new List<RecipeMisc>();
}
