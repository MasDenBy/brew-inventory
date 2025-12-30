using BrewInventory.App.Models;

namespace BrewInventory.App.Data.Entities;

public class Yeast : Inventory
{
    public string Labaratory { get; set; } = null!;
    public YeastType Type { get; set; }
    public YeastForm Form { get; set; }
}
