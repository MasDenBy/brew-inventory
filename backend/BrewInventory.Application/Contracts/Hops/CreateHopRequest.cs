namespace BrewInventory.Application.Contracts.Hops;

public record CreateHopRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Origin, string? Type, double AlphaAcid, int? HarvestYear);
