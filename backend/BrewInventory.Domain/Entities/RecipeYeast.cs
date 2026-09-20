using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class RecipeYeast
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public string Name { get; set; } = null!;
    public double Amount { get; set; }
    public string? Laboratory { get; set; }
    public YeastType Type { get; set; }
    public YeastForm Form { get; set; }
    public double? Attenuation { get; set; }
    public string? Unit { get; set; }
}
