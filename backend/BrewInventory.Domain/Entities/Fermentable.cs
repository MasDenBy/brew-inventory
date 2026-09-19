using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class Fermentable : Inventory
{
    public string? Supplier { get; set; }
    public string? Origin { get; set; }
    public FermentableType Type { get; set; }
    public double Color { get; set; }
    public string? GrainCategory { get; set; }
    public int? Percentage { get; set; }
    public double Lovibond { get; set; }
}
