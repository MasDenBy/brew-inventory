namespace BrewInventory.App.Data.Entities;

public abstract class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateOnly? BestBefore { get; set; }
    public string? BrewfatherId { get; set; }
}
