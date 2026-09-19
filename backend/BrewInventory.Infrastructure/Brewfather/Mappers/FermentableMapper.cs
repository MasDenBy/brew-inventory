using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class FermentableMapper
{
    public static Fermentable ToEntity(BrewfatherFermentable bf)
    {
        ArgumentNullException.ThrowIfNull(bf);

        return new()
        {
            Name = bf.Name,
            Amount = bf.Inventory ?? 0,
            BrewfatherId = bf.Id,
            Supplier = bf.Supplier,
            Origin = bf.Origin,
            Type = bf.Type,
            Color = bf.Color,
            GrainCategory = bf.GrainCategory,
            Percentage = bf.Percentage,
            Lovibond = bf.Lovibond
        };
    }

    public static void UpdateEntity(Fermentable existing, BrewfatherFermentable bf)
    {
        ArgumentNullException.ThrowIfNull(existing);
        ArgumentNullException.ThrowIfNull(bf);

        existing.Name = bf.Name;
        existing.Amount = bf.Inventory ?? 0;
        existing.Supplier = bf.Supplier;
        existing.Origin = bf.Origin;
        existing.Type = bf.Type;
        existing.Color = bf.Color;
        existing.GrainCategory = bf.GrainCategory;
        existing.Percentage = bf.Percentage;
        existing.Lovibond = bf.Lovibond;
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
        Supplier = bf.supplier,
        Origin = bf.origin,
        Type = FromBrewfatherType(bf.type),
        Color = bf.color ?? 0
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
