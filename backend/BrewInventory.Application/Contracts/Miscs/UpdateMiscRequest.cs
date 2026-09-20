using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Miscs;

public record UpdateMiscRequest(string Name, double Amount, InventoryUnit Unit, MiscType Type, string Use, string? BrewfatherId);
