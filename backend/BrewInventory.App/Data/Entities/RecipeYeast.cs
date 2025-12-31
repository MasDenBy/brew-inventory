namespace BrewInventory.App.Data.Entities;

public class RecipeYeast
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public int YeastId { get; set; }
    public Yeast Yeast { get; set; } = null!;

    public decimal Amount { get; set; }
}
