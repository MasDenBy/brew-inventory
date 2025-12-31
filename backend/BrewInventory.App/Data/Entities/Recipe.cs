namespace BrewInventory.App.Data.Entities;

public class Recipe
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? BrewfatherId { get; set; }
    
    public ICollection<RecipeFermentable> RecipeFermentables { get; set; } = new List<RecipeFermentable>();
    public ICollection<RecipeHop> RecipeHops { get; set; } = new List<RecipeHop>();
    public ICollection<RecipeYeast> RecipeYeasts { get; set; } = new List<RecipeYeast>();
    public ICollection<RecipeMisc> RecipeMiscs { get; set; } = new List<RecipeMisc>();
}
