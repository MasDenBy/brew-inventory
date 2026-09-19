using BrewInventory.Domain.Entities;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class RecipeMapper
{
    public static Recipe ToEntity(
        BrewfatherRecipe bfRecipe,
        Dictionary<string, Fermentable> fermentableLookup,
        Dictionary<string, Hop> hopLookup,
        Dictionary<string, Yeast> yeastLookup,
        Dictionary<string, Misc> miscLookup)
    {
        var recipe = new Recipe
        {
            Name = bfRecipe.name,
            BrewfatherId = bfRecipe._id
        };

        if (bfRecipe.fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.fermentables)
            {
                if (fermentableLookup.TryGetValue(bfFermentable._id, out var fermentable))
                {
                    recipe.RecipeFermentables.Add(new RecipeFermentable
                    {
                        FermentableId = fermentable.Id,
                        Amount = (double)bfFermentable.amount
                    });
                }
            }
        }

        if (bfRecipe.hops != null)
        {
            foreach (var bfHop in bfRecipe.hops)
            {
                if (hopLookup.TryGetValue(bfHop._id, out var hop))
                {
                    recipe.RecipeHops.Add(new RecipeHop
                    {
                        HopId = hop.Id,
                        Amount = (double)bfHop.amount
                    });
                }
            }
        }

        if (bfRecipe.yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.yeasts)
            {
                if (yeastLookup.TryGetValue(bfYeast._id, out var yeast))
                {
                    recipe.RecipeYeasts.Add(new RecipeYeast
                    {
                        YeastId = yeast.Id,
                        Amount = (double)bfYeast.amount
                    });
                }
            }
        }

        if (bfRecipe.miscs != null)
        {
            foreach (var bfMisc in bfRecipe.miscs)
            {
                if (miscLookup.TryGetValue(bfMisc._id, out var misc))
                {
                    recipe.RecipeMiscs.Add(new RecipeMisc
                    {
                        MiscId = misc.Id,
                        Amount = (double)bfMisc.amount
                    });
                }
            }
        }

        return recipe;
    }

    public static void UpdateEntity(
        Recipe existingRecipe,
        BrewfatherRecipe bfRecipe,
        Dictionary<string, Fermentable> fermentableLookup,
        Dictionary<string, Hop> hopLookup,
        Dictionary<string, Yeast> yeastLookup,
        Dictionary<string, Misc> miscLookup)
    {
        existingRecipe.Name = bfRecipe.name;

        existingRecipe.RecipeFermentables.Clear();
        existingRecipe.RecipeHops.Clear();
        existingRecipe.RecipeYeasts.Clear();
        existingRecipe.RecipeMiscs.Clear();

        if (bfRecipe.fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.fermentables)
            {
                if (fermentableLookup.TryGetValue(bfFermentable._id, out var fermentable))
                {
                    existingRecipe.RecipeFermentables.Add(new RecipeFermentable
                    {
                        FermentableId = fermentable.Id,
                        Amount = (double)bfFermentable.amount
                    });
                }
            }
        }

        if (bfRecipe.hops != null)
        {
            foreach (var bfHop in bfRecipe.hops)
            {
                if (hopLookup.TryGetValue(bfHop._id, out var hop))
                {
                    existingRecipe.RecipeHops.Add(new RecipeHop
                    {
                        HopId = hop.Id,
                        Amount = (double)bfHop.amount
                    });
                }
            }
        }

        if (bfRecipe.yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.yeasts)
            {
                if (yeastLookup.TryGetValue(bfYeast._id, out var yeast))
                {
                    existingRecipe.RecipeYeasts.Add(new RecipeYeast
                    {
                        YeastId = yeast.Id,
                        Amount = (double)bfYeast.amount
                    });
                }
            }
        }

        if (bfRecipe.miscs != null)
        {
            foreach (var bfMisc in bfRecipe.miscs)
            {
                if (miscLookup.TryGetValue(bfMisc._id, out var misc))
                {
                    existingRecipe.RecipeMiscs.Add(new RecipeMisc
                    {
                        MiscId = misc.Id,
                        Amount = (double)bfMisc.amount
                    });
                }
            }
        }
    }

    public static BrewfatherCreateRecipeRequest ToBrewfatherRequest(Recipe recipe)
    {
        return new BrewfatherCreateRecipeRequest(
            recipe.Name,
            "All Grain",
            recipe.RecipeFermentables.Select(rf => new BrewfatherRecipeFermentable(
                rf.Fermentable.BrewfatherId!,
                (double)rf.Amount,
                rf.Fermentable.Name,
                FermentableMapper.ToBrewfatherType(rf.Fermentable.Type),
                rf.Fermentable.Supplier,
                rf.Fermentable.Origin,
                rf.Fermentable.Color,
                null
            )).ToList(),
            recipe.RecipeHops.Select(rh => new BrewfatherRecipeHop(
                rh.Hop.BrewfatherId!,
                (double)rh.Amount,
                rh.Hop.Name,
                rh.Hop.AlphaAcid,
                HopMapper.ToBrewfatherType(rh.Hop.Type),
                rh.Hop.Origin,
                "Boil",
                60
            )).ToList(),
            recipe.RecipeMiscs.Select(rm => new BrewfatherRecipeMisc(
                rm.Misc.BrewfatherId!,
                (double)rm.Amount,
                rm.Misc.Name,
                MiscMapper.ToBrewfatherType(rm.Misc.Type),
                rm.Misc.Unit,
                "Boil",
                0
            )).ToList(),
            recipe.RecipeYeasts.Select(ry => new BrewfatherRecipeYeast(
                ry.Yeast.BrewfatherId!,
                (double)ry.Amount,
                ry.Yeast.Name,
                string.IsNullOrWhiteSpace(ry.Yeast.Laboratory) ? null : ry.Yeast.Laboratory,
                YeastMapper.ToBrewfatherType(ry.Yeast.Type),
                YeastMapper.ToBrewfatherForm(ry.Yeast.Form),
                null,
                "pkg"
            )).ToList()
        );
    }

    public static ICollection<string> GetIngredientsWithoutBrewfatherId(Recipe recipe)
    {
        var missing = new List<string>();

        foreach (var rf in recipe.RecipeFermentables)
        {
            if (string.IsNullOrWhiteSpace(rf.Fermentable.BrewfatherId))
            {
                missing.Add($"fermentable '{rf.Fermentable.Name}'");
            }
        }

        foreach (var rh in recipe.RecipeHops)
        {
            if (string.IsNullOrWhiteSpace(rh.Hop.BrewfatherId))
            {
                missing.Add($"hop '{rh.Hop.Name}'");
            }
        }

        foreach (var ry in recipe.RecipeYeasts)
        {
            if (string.IsNullOrWhiteSpace(ry.Yeast.BrewfatherId))
            {
                missing.Add($"yeast '{ry.Yeast.Name}'");
            }
        }

        foreach (var rm in recipe.RecipeMiscs)
        {
            if (string.IsNullOrWhiteSpace(rm.Misc.BrewfatherId))
            {
                missing.Add($"misc '{rm.Misc.Name}'");
            }
        }

        return missing;
    }
}
