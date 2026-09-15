using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class Yeast : Inventory
{
    public string Labaratory { get; set; } = null!;
    public YeastType Type { get; set; }
    public YeastForm Form { get; set; }
}
