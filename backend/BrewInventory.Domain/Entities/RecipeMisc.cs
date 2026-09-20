using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class RecipeMisc
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public string Name { get; set; } = null!;
    public double Amount { get; set; }
    public MiscType Type { get; set; }
    public string? Unit { get; set; }
    public string? Use { get; set; }
    public double? Time { get; set; }
}
