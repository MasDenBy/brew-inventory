namespace BrewInventory.App.Models.Contracts;

public record CreateHopRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Origin, string? Type, double AlphaAcid, int? HarvestYear);
public record UpdateHopRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Origin, string? Type, double AlphaAcid, int? HarvestYear);
public record HopResponse(int Id, string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Origin, string? Type, double AlphaAcid, int? HarvestYear);
