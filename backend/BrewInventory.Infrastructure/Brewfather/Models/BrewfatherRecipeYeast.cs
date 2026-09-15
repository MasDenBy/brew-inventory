namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeYeast(
    string _id,
    double amount,
    string name,
    string? laboratory,
    string? type,
    string? form,
    double? attenuation,
    string? unit
);
