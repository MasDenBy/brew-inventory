namespace BrewInventory.Application.Contracts.Hops;

public record UpdateHopRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Origin, string? Type, double AlphaAcid, int? HarvestYear);
