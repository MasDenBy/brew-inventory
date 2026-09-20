using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Domain.Entities;

public class Yeast : Inventory
{
    public string Laboratory { get; set; } = null!;
    public YeastType Type { get; set; }
    public YeastForm Form { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string? ProductId { get; set; }
}
