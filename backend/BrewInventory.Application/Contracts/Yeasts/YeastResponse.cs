namespace BrewInventory.Application.Contracts.Yeasts;

public record YeastResponse(int Id, string Name, double Amount, string? BrewfatherId, string Laboratory, string? Type, string? Form, string Unit, string? ProductId);
