using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class YeastMapper
{
    public static Yeast ToEntity(BrewfatherYeast bf)
    {
        ArgumentNullException.ThrowIfNull(bf);

        return new()
        {
            Name = bf.Name,
            Amount = bf.Inventory,
            BrewfatherId = bf.Id,
            Type = bf.Type,
            Laboratory = bf.Laboratory ?? string.Empty,
            Form = bf.Form,
            Unit = bf.Unit,
            ProductId = bf.ProductId,
        };
    }

    public static void UpdateEntity(Yeast existing, BrewfatherYeast bf)
    {
        ArgumentNullException.ThrowIfNull(existing);
        ArgumentNullException.ThrowIfNull(bf);

        existing.Name = bf.Name;
        existing.Amount = bf.Inventory;
        existing.Type = bf.Type;
        existing.Form = bf.Form;
        existing.Laboratory = bf.Laboratory ?? existing.Laboratory;
        existing.Unit = bf.Unit;
        existing.ProductId = bf.ProductId;
    }

    public static string ToBrewfatherType(YeastType type) => type switch
    {
        YeastType.Ale => "Ale",
        YeastType.Lager => "Lager",
        YeastType.Hybrid => "Hybrid",
        YeastType.Wheat => "Wheat",
        YeastType.Wine => "Wine",
        YeastType.Champagne => "Champagne",
        _ => "Other"
    };

    public static string ToBrewfatherForm(YeastForm form) => form switch
    {
        YeastForm.Dry => "Dry",
        YeastForm.Liquid => "Liquid",
        YeastForm.Slurry => "Slurry",
        YeastForm.Culture => "Culture",
        _ => "Liquid"
    };

    public static Yeast ToEntityFromRecipe(Models.BrewfatherRecipeYeast bf) => new()
    {
        Name = bf.Name,
        Amount = 0,
        BrewfatherId = bf.Id,
        Type = FromBrewfatherType(bf.Type.ToString()),
        Laboratory = string.IsNullOrWhiteSpace(bf.Laboratory) ? "Unknown" : bf.Laboratory,
        Form = FromBrewfatherForm(bf.Form.ToString())
    };

    public static YeastType FromBrewfatherType(string? brewfatherType) => brewfatherType?.ToLowerInvariant() switch
    {
        "ale" => YeastType.Ale,
        "lager" => YeastType.Lager,
        "hybrid" => YeastType.Hybrid,
        "wheat" => YeastType.Wheat,
        "wine" => YeastType.Wine,
        "champagne" => YeastType.Champagne,
        _ => YeastType.Other
    };

    public static YeastForm FromBrewfatherForm(string? brewfatherForm) => brewfatherForm?.ToLowerInvariant() switch
    {
        "dry" => YeastForm.Dry,
        "liquid" => YeastForm.Liquid,
        "slurry" => YeastForm.Slurry,
        "culture" => YeastForm.Culture,
        _ => YeastForm.Liquid
    };
}
