namespace BrewInventory.Application.Contracts.Yeasts;

public record YeastResponse(int Id, string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string Laboratory, string? Type, string? Form);
