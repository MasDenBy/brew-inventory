using BrewInventory.Domain.Entities;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class RecipeMapper
{
    public static Recipe ToEntity(BrewfatherRecipe bfRecipe)
    {
        ArgumentNullException.ThrowIfNull(bfRecipe);

        var recipe = new Recipe
        {
            Name = bfRecipe.Name,
            BrewfatherId = bfRecipe.Id,
            Style = bfRecipe.Style?.Name,
            Notes = bfRecipe.Notes
        };

        recipe.AddFermentables(ToEntities(bfRecipe.Fermentables, ToFermentableEntity));
        recipe.AddHops(ToEntities(bfRecipe.Hops, ToHopEntity));
        recipe.AddYeasts(ToEntities(bfRecipe.Yeasts, ToYeastEntity));
        recipe.AddMiscs(ToEntities(bfRecipe.Miscs, ToMiscEntity));

        return recipe;
    }

    public static void UpdateEntity(Recipe existingRecipe, BrewfatherRecipe bfRecipe)
    {
        ArgumentNullException.ThrowIfNull(existingRecipe);
        ArgumentNullException.ThrowIfNull(bfRecipe);

        existingRecipe.Name = bfRecipe.Name;
        existingRecipe.Style = bfRecipe.Style?.Name;
        existingRecipe.Notes = bfRecipe.Notes;

        existingRecipe.RecipeFermentables.Clear();
        existingRecipe.RecipeHops.Clear();
        existingRecipe.RecipeYeasts.Clear();
        existingRecipe.RecipeMiscs.Clear();

        existingRecipe.AddFermentables(ToEntities(bfRecipe.Fermentables, ToFermentableEntity));
        existingRecipe.AddHops(ToEntities(bfRecipe.Hops, ToHopEntity));
        existingRecipe.AddYeasts(ToEntities(bfRecipe.Yeasts, ToYeastEntity));
        existingRecipe.AddMiscs(ToEntities(bfRecipe.Miscs, ToMiscEntity));
    }

    public static BrewfatherCreateRecipeRequest ToBrewfatherRequest(Recipe recipe)
    {
        ArgumentNullException.ThrowIfNull(recipe);

        return new BrewfatherCreateRecipeRequest(
            recipe.Name,
            "All Grain",
            recipe.RecipeFermentables.Select(rf => new BrewfatherRecipeFermentable
            {
                Amount = rf.Amount,
                Name = rf.Name,
                Type = rf.Type,
                Supplier = rf.Supplier,
                Origin = rf.Origin,
                Color = rf.Color,
                Potential = rf.Potential,
            }).ToList(),
            recipe.RecipeHops.Select(rh => new BrewfatherRecipeHop
            {
                Amount = rh.Amount,
                Name = rh.Name,
                Alpha = rh.Alpha,
                Type = rh.Type,
                Origin = rh.Origin,
                Use = rh.Use,
                Time = rh.Time,
            }).ToList(),
            recipe.RecipeMiscs.Select(rm => new BrewfatherRecipeMisc
            {
                Amount = rm.Amount,
                Name = rm.Name,
                Type = rm.Type,
                Unit = rm.Unit,
                Use = rm.Use,
                Time = rm.Time,
            }).ToList(),
            recipe.RecipeYeasts.Select(ry => new BrewfatherRecipeYeast
            {
                Amount = ry.Amount,
                Name = ry.Name,
                Laboratory = ry.Laboratory,
                Type = ry.Type,
                Form = ry.Form,
                Attenuation = ry.Attenuation,
                Unit = ry.Unit,
            }).ToList(),
            recipe.Notes
        );
    }

    private static List<T> ToEntities<T, TSource>(IEnumerable<TSource>? source, Func<TSource, T> mapper)
    {
        return source?.Select(mapper).ToList() ?? [];
    }

    private static RecipeFermentable ToFermentableEntity(BrewfatherRecipeFermentable bf) => new()
    {
        Name = bf.Name,
        Amount = bf.Amount,
        Type = bf.Type,
        Supplier = bf.Supplier,
        Origin = bf.Origin,
        Color = bf.Color,
        Potential = bf.Potential,
    };

    private static RecipeHop ToHopEntity(BrewfatherRecipeHop bf) => new()
    {
        Name = bf.Name,
        Amount = bf.Amount,
        Alpha = bf.Alpha,
        Type = bf.Type,
        Origin = bf.Origin,
        Use = bf.Use,
        Time = bf.Time,
    };

    private static RecipeYeast ToYeastEntity(BrewfatherRecipeYeast bf) => new()
    {
        Name = bf.Name,
        Amount = bf.Amount,
        Laboratory = bf.Laboratory,
        Type = bf.Type,
        Form = bf.Form,
        Attenuation = bf.Attenuation,
        Unit = bf.Unit,
    };

    private static RecipeMisc ToMiscEntity(BrewfatherRecipeMisc bf) => new()
    {
        Name = bf.Name,
        Amount = bf.Amount,
        Type = bf.Type,
        Unit = bf.Unit,
        Use = bf.Use,
        Time = bf.Time,
    };
}
