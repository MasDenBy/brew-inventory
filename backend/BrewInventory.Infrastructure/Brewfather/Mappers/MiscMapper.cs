using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class MiscMapper
{
    public static Misc ToEntity(BrewfatherMisc bf)
    {
        ArgumentNullException.ThrowIfNull(bf);

        return new()
        {
            Name = bf.Name,
            Amount = bf.Inventory ?? 0,
            BrewfatherId = bf.Id,
            Type = bf.Type,
            Unit = bf.Unit,
            Use = bf.Use
        };
    }

    public static void UpdateEntity(Misc existing, BrewfatherMisc bf)
    {
        ArgumentNullException.ThrowIfNull(existing);
        ArgumentNullException.ThrowIfNull(bf);

        existing.Name = bf.Name;
        existing.Amount = bf.Inventory ?? 0;
        existing.Type = bf.Type;
        existing.Unit = bf.Unit;
        existing.Use = bf.Use;
    }

    public static string ToBrewfatherType(MiscType type) => type switch
    {
        MiscType.Spice => "Spice",
        MiscType.Herb => "Herb",
        MiscType.Flavor => "Flavoring",
        MiscType.Fining => "Fining",
        MiscType.WaterAgent => "Water Agent",
        _ => "Other",
    };

    public static string ToBrewfatherUnit(InventoryUnit unit) => unit switch
    {
        InventoryUnit.Kilograms => "kg",
        InventoryUnit.Liters => "l",
        InventoryUnit.Milliliters => "ml",
        InventoryUnit.Packages => "pkg",
        InventoryUnit.Tablets => "items",
        _ => "g",
    };

    public static Misc ToEntityFromRecipe(Models.BrewfatherRecipeMisc bf) => new()
    {
        Name = bf.name,
        Amount = 0,
        BrewfatherId = bf._id,
        Type = FromBrewfatherType(bf.type),
        Unit = bf.unit ?? "g",
        Use = bf.use ?? "",
    };

    public static MiscType FromBrewfatherType(string? brewfatherType) => brewfatherType?.ToUpperInvariant() switch
    {
        "SPICE" => MiscType.Spice,
        "HERB" => MiscType.Herb,
        "FLAVOR" or "FLAVORING" => MiscType.Flavor,
        "FINING" => MiscType.Fining,
        "WATER AGENT" or "WATER" => MiscType.WaterAgent,
        _ => MiscType.Other,
    };
}
