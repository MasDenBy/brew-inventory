namespace BrewInventory.App.Data.Entities;

public class RecipeHop
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public int HopId { get; set; }
    public Hop Hop { get; set; } = null!;

    public decimal Amount { get; set; }
}
