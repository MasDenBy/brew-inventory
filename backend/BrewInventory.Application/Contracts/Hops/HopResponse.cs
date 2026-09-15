namespace BrewInventory.Application.Contracts.Hops;

public record HopResponse(int Id, string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Origin, string? Type, double AlphaAcid, int? HarvestYear);
