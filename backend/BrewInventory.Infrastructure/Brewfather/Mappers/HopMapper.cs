using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class HopMapper
{
    public static Hop ToEntity(BrewfatherHop bf) => new()
    {
        Name = bf.name,
        Amount = bf.inventory,
        BrewfatherId = bf._id,
        AlphaAcid = bf.alpha,
        Type = FromBrewfatherType(bf.type),
        Origin = null,
        HarvestYear = null
    };

    public static void UpdateEntity(Hop existing, BrewfatherHop bf)
    {
        existing.Name = bf.name;
        existing.Amount = bf.inventory;
        existing.AlphaAcid = bf.alpha;
        existing.Type = FromBrewfatherType(bf.type);
    }

    public static string ToBrewfatherType(HopType type) => type switch
    {
        HopType.Pellet => "Pellet",
        HopType.Whole => "Whole",
        HopType.Cryo => "Cryo",
        HopType.CO2Extract => "Co2 Extract",
        _ => "Pellet"
    };

    public static Hop ToEntityFromRecipe(Models.BrewfatherRecipeHop bf) => new()
    {
        Name = bf.name,
        Amount = 0,
        BrewfatherId = bf._id,
        AlphaAcid = bf.alpha ?? 0,
        Type = FromBrewfatherType(bf.type),
        Origin = string.IsNullOrWhiteSpace(bf.origin) ? null : bf.origin,
        HarvestYear = null
    };

    public static HopType FromBrewfatherType(string? brewfatherType) => brewfatherType?.ToLowerInvariant() switch
    {
        "pellet" => HopType.Pellet,
        "whole" or "leaf" => HopType.Whole,
        "cryo" => HopType.Cryo,
        "extract" or "co2 extract" => HopType.CO2Extract,
        _ => HopType.Pellet
    };
}
