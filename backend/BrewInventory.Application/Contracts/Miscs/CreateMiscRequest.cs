using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Miscs;

public record CreateMiscRequest(string Name, double Amount, InventoryUnit Unit, MiscType Type, string? BrewfatherId);
