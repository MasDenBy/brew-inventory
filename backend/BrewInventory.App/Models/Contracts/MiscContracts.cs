namespace BrewInventory.App.Models.Contracts;

public record CreateMiscRequest(string Name, decimal Amount, InventoryUnit Unit, MiscType Type, DateOnly? BestBefore, string? BrewfatherId);
public record UpdateMiscRequest(string Name, decimal Amount, InventoryUnit Unit, MiscType Type, DateOnly? BestBefore, string? BrewfatherId);
public record MiscResponse(int Id, string Name, decimal Amount, InventoryUnit Unit, MiscType Type, DateOnly? BestBefore, string? BrewfatherId);
