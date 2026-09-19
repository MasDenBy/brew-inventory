using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class MiscMapper
{
    public static Misc ToEntity(BrewfatherMisc bf) => new()
    {
        Name = bf.name,
        Amount = bf.inventory,
        BrewfatherId = bf._id,
        Type = FromBrewfatherType(bf.type),
        Unit = InventoryUnit.Grams
    };

    public static void UpdateEntity(Misc existing, BrewfatherMisc bf)
    {
        existing.Name = bf.name;
        existing.Amount = bf.inventory;
        existing.Type = FromBrewfatherType(bf.type);
    }

    public static string ToBrewfatherType(MiscType type) => type switch
    {
        MiscType.Spice => "Spice",
        MiscType.Herb => "Herb",
        MiscType.Fruit => "Fruit",
        MiscType.Flavor => "Flavoring",
        MiscType.Fining => "Fining",
        MiscType.WaterAgent => "Water Agent",
        _ => "Other"
    };

    public static string ToBrewfatherUnit(InventoryUnit unit) => unit switch
    {
        InventoryUnit.Kilograms => "kg",
        InventoryUnit.Liters => "l",
        InventoryUnit.Milliliters => "ml",
        InventoryUnit.Packages => "pkg",
        InventoryUnit.Tablets => "items",
        _ => "g"
    };

    public static Misc ToEntityFromRecipe(Models.BrewfatherRecipeMisc bf) => new()
    {
        Name = bf.name,
        Amount = 0,
        BrewfatherId = bf._id,
        Type = FromBrewfatherType(bf.type),
        Unit = FromBrewfatherUnit(bf.unit)
    };

    public static MiscType FromBrewfatherType(string? brewfatherType) => brewfatherType?.ToLowerInvariant() switch
    {
        "spice" => MiscType.Spice,
        "herb" => MiscType.Herb,
        "fruit" => MiscType.Fruit,
        "flavor" or "flavoring" => MiscType.Flavor,
        "fining" => MiscType.Fining,
        "water agent" or "water" => MiscType.WaterAgent,
        _ => MiscType.Other
    };

    public static InventoryUnit FromBrewfatherUnit(string? brewfatherUnit) => brewfatherUnit?.ToLowerInvariant() switch
    {
        "g" or "gram" or "grams" => InventoryUnit.Grams,
        "kg" or "kilogram" or "kilograms" => InventoryUnit.Kilograms,
        "ml" or "milliliter" or "milliliters" => InventoryUnit.Milliliters,
        "l" or "liter" or "liters" => InventoryUnit.Liters,
        "pkg" or "package" or "packages" => InventoryUnit.Packages,
        "tablet" or "tablets" => InventoryUnit.Tablets,
        _ => InventoryUnit.Grams
    };
}
