namespace BrewInventory.App.Models.Contracts;

public record CreateYeastRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string Laboratory, string? Type, string? Form);
public record UpdateYeastRequest(string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string Laboratory, string? Type, string? Form);
public record YeastResponse(int Id, string Name, decimal Amount, DateOnly? BestBefore, string? BrewfatherId, string Laboratory, string? Type, string? Form);
