namespace BrewInventory.Application.Contracts.Fermentables;

public record CreateFermentableRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Supplier, string? Origin, string? Type, double Color);
