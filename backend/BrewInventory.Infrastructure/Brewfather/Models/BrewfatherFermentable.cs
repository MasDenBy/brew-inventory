namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherFermentable(
    string _id,
    double? attenuation,
    double inventory,
    string name,
    string supplier,
    string type,
    string use
);
