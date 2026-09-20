using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class RecipeHop
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public string Name { get; set; } = null!;
    public double Amount { get; set; }
    public double? Alpha { get; set; }
    public HopType Type { get; set; }
    public string? Origin { get; set; }
    public HopUse Use { get; set; }
    public double? Time { get; set; }
}
