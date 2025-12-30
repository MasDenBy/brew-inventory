namespace BrewInventory.App.Models;

public record BrewfatherFermentable(
    string _id,
    double? attenuation,
    double inventory,
    string name,
    string supplier,
    string type,
    string use
);

public record BrewfatherHop(
    string _id,
    double alpha,
    double inventory,
    string name,
    string type,
    string use
);

public record BrewfatherYeast(
    string _id,
    double? attenuation,
    double inventory,
    string name,
    string type
);

public record BrewfatherMisc(
    string _id,
    double inventory,
    string name,
    string type,
    string use
);
