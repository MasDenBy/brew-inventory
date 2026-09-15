namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherMisc(
    string _id,
    double inventory,
    string name,
    string type,
    string use
);
