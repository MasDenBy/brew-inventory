namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherYeast(
    string _id,
    double? attenuation,
    double inventory,
    string name,
    string type
);
