namespace BrewInventory.Application.Contracts.Hops;

public record HopResponse(int Id, string Name, double Amount, string? BrewfatherId, string? Origin, string? Type, double AlphaAcid, int? HarvestYear);
