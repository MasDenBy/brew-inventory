namespace BrewInventory.App.Data.Entities;

public class RecipeMisc
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public int MiscId { get; set; }
    public Misc Misc { get; set; } = null!;

    public decimal Amount { get; set; }
}
