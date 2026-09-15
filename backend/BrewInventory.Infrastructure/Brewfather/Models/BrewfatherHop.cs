namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherHop(
    string _id,
    double alpha,
    double inventory,
    string name,
    string type,
    string use
);
