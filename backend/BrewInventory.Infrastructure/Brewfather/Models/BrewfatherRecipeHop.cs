namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeHop(
    string _id,
    double amount,
    string name,
    double? alpha,
    string? type,
    string? origin,
    string? use,
    double? time
);
