using BrewInventory.App.Models;

namespace BrewInventory.App.Data.Entities;

public class Hop : Inventory
{
    public string? Origin { get; set; }
    public HopType Type { get; set; }
    public double AlphaAcid { get; set; }
    public int? HarvestYear { get; set; }
}
