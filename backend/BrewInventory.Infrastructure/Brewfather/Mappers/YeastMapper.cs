using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class YeastMapper
{
    public static Yeast ToEntity(BrewfatherYeast bf) => new()
    {
        Name = bf.name,
        Amount = (decimal)bf.inventory,
        BrewfatherId = bf._id,
        Type = FromBrewfatherType(bf.type),
        Labaratory = string.Empty,
        Form = YeastForm.Liquid,
        BestBefore = null
    };

    public static void UpdateEntity(Yeast existing, BrewfatherYeast bf)
    {
        existing.Name = bf.name;
        existing.Amount = (decimal)bf.inventory;
        existing.Type = FromBrewfatherType(bf.type);
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
        Name = bf.name,
        Amount = 0,
        BrewfatherId = bf._id,
        Type = FromBrewfatherType(bf.type),
        Labaratory = string.IsNullOrWhiteSpace(bf.laboratory) ? "Unknown" : bf.laboratory,
        Form = FromBrewfatherForm(bf.form),
        BestBefore = null
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
