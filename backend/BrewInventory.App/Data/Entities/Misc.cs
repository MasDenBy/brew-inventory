using BrewInventory.App.Models;

namespace BrewInventory.App.Data.Entities;

public class Misc : Inventory
{
    public InventoryUnit Unit { get; set; }
    public MiscType Type { get; set; }
}
