namespace BrewInventory.Application.Contracts.Yeasts;

public record CreateYeastRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string Laboratory, string? Type, string? Form);
