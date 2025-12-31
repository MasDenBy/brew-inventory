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

// Recipe DTOs
public record BrewfatherRecipe(
    string _id,
    string name,
    string? author,
    string? type,
    BrewfatherRecipeEquipment? equipment,
    BrewfatherRecipeStyle? style,
    List<BrewfatherRecipeFermentable>? fermentables,
    List<BrewfatherRecipeHop>? hops,
    List<BrewfatherRecipeMisc>? miscs,
    List<BrewfatherRecipeYeast>? yeasts
);

public record BrewfatherRecipeEquipment(
    string? name
);

public record BrewfatherRecipeStyle(
    string? name
);

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

public record BrewfatherRecipeMisc(
    string _id,
    double amount,
    string name,
    string? type,
    string? unit,
    string? use,
    double? time
);

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
