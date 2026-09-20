namespace BrewInventory.Domain.Entities;

public abstract class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double Amount { get; set; }
    public string? BrewfatherId { get; set; }
}
