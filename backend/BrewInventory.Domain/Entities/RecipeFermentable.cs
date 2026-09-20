using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class RecipeFermentable
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; } = null!;

    public string Name { get; set; } = null!;
    public double Amount { get; set; }
    public FermentableType Type { get; set; }
    public string? Supplier { get; set; }
    public string? Origin { get; set; }
    public double? Color { get; set; }
    public double? Potential { get; set; }
}
