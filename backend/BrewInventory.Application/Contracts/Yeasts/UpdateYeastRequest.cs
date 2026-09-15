namespace BrewInventory.Application.Contracts.Yeasts;

public record UpdateYeastRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string Laboratory, string? Type, string? Form);
