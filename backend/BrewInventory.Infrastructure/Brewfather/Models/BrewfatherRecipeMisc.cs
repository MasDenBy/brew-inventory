namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeMisc(
    string _id,
    double amount,
    string name,
    string? type,
    string? unit,
    string? use,
    double? time
);
