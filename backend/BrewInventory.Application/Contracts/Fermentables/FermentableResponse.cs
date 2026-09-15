namespace BrewInventory.Application.Contracts.Fermentables;

public record FermentableResponse(int Id, string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string? Supplier, string? Origin, string? Type, double Color);
