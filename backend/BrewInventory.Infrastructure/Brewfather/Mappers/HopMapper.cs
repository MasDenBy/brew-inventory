using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class HopMapper
{
    public static Hop ToEntity(BrewfatherHop bf)
    {
        ArgumentNullException.ThrowIfNull(bf);

        return new()
        {
            Name = bf.Name,
            Amount = bf.Inventory ?? 0,
            BrewfatherId = bf.Id,
            AlphaAcid = bf.Alpha,
            Type = bf.Type,
            Origin = bf.Origin,
            HarvestYear = bf.Year
        };
    }

    public static void UpdateEntity(Hop existing, BrewfatherHop bf)
    {
        ArgumentNullException.ThrowIfNull(existing);
        ArgumentNullException.ThrowIfNull(bf);

        existing.Name = bf.Name;
        existing.Amount = bf.Inventory ?? 0;
        existing.AlphaAcid = bf.Alpha;
        existing.Type = bf.Type;
        existing.Origin = bf.Origin;
        existing.HarvestYear = bf.Year;
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
        Name = bf.Name,
        Amount = 0,
        BrewfatherId = bf.Id,
        AlphaAcid = bf.Alpha ?? 0,
        Type = FromBrewfatherType(bf.Type.ToString()),
        Origin = string.IsNullOrWhiteSpace(bf.Origin) ? null : bf.Origin,
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
