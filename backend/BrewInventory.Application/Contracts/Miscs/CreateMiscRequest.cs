using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Miscs;

public record CreateMiscRequest(string Name, decimal Amount, InventoryUnit Unit, MiscType Type, DateOnly? BestBefore, string? BrewfatherId);
