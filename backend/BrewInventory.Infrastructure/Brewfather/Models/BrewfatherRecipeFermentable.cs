namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeFermentable(
    string _id,
    double amount,
    string name,
    string? type,
    string? supplier,
    string? origin,
    double? color,
    double? potential
);
