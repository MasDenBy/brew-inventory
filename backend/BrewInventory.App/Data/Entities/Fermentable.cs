using BrewInventory.App.Models;

namespace BrewInventory.App.Data.Entities;

public class Fermentable : Inventory
{
    public string? Supplier { get; set; }
    public string? Origin { get; set; }
    public FermentableType Type { get; set; }
    public double Color { get; set; }
}
