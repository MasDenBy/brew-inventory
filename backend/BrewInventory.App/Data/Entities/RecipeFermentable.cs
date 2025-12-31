namespace BrewInventory.App.Data.Entities;

public class RecipeFermentable
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public int FermentableId { get; set; }
    public Fermentable Fermentable { get; set; } = null!;

    public decimal Amount { get; set; }
}
