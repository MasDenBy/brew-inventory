namespace BrewInventory.App.Models.Contracts;

public record CreateFermentableRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Supplier, string? Origin, string? Type, double Color);
public record UpdateFermentableRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Supplier, string? Origin, string? Type, double Color);
public record FermentableResponse(int Id, string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Supplier, string? Origin, string? Type, double Color);
