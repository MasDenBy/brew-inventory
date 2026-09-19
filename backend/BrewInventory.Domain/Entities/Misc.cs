using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class Misc : Inventory
{
    public string Unit { get; set; } = null!;
    public MiscType Type { get; set; }
    public string Use { get; set; } = null!;
}
