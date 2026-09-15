using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class Misc : Inventory
{
    public InventoryUnit Unit { get; set; }
    public MiscType Type { get; set; }
}
