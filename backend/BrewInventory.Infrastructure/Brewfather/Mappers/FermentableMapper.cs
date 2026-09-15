using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class FermentableMapper
{
    public static Fermentable ToEntity(BrewfatherFermentable bf) => new()
    {
        Name = bf.name,
        Amount = (decimal)bf.inventory,
        BrewfatherId = bf._id,
        Supplier = string.IsNullOrWhiteSpace(bf.supplier) ? null : bf.supplier,
        Type = FromBrewfatherType(bf.type),
        Color = 0,
        Origin = null,
        BestBefore = null
    };

    public static void UpdateEntity(Fermentable existing, BrewfatherFermentable bf)
    {
        existing.Name = bf.name;
        existing.Amount = (decimal)bf.inventory;
        existing.Supplier = string.IsNullOrWhiteSpace(bf.supplier) ? null : bf.supplier;
        existing.Type = FromBrewfatherType(bf.type);
    }

    public static string ToBrewfatherType(FermentableType type) => type switch
    {
        FermentableType.Grain => "Grain",
        FermentableType.Sugar => "Sugar",
        FermentableType.LiquidExtract => "Extract",
        FermentableType.DryExtract => "Dry Extract",
        FermentableType.Adjunct => "Adjunct",
        _ => "Other"
    };

    public static Fermentable ToEntityFromRecipe(Models.BrewfatherRecipeFermentable bf) => new()
    {
        Name = bf.name,
        Amount = 0,
        BrewfatherId = bf._id,
        Supplier = string.IsNullOrWhiteSpace(bf.supplier) ? null : bf.supplier,
        Origin = string.IsNullOrWhiteSpace(bf.origin) ? null : bf.origin,
        Type = FromBrewfatherType(bf.type),
        Color = bf.color ?? 0,
        BestBefore = null
    };

    public static FermentableType FromBrewfatherType(string? brewfatherType) => brewfatherType?.ToLowerInvariant() switch
    {
        "grain" => FermentableType.Grain,
        "sugar" => FermentableType.Sugar,
        "extract" => FermentableType.LiquidExtract,
        "dry extract" => FermentableType.DryExtract,
        "adjunct" => FermentableType.Adjunct,
        _ => FermentableType.Other
    };
}
