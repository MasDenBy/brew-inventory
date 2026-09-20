using BrewInventory.Domain.Entities;
using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather.Mappers;

public static class RecipeMapper
{
    public static Recipe ToEntity(BrewfatherRecipe bfRecipe)
    {
        var recipe = new Recipe
        {
            Name = bfRecipe.Name,
            BrewfatherId = bfRecipe.Id,
            Style = bfRecipe.Style?.Name
        };

        if (bfRecipe.Fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.Fermentables)
            {
                recipe.RecipeFermentables.Add(new RecipeFermentable
                {
                    Name = bfFermentable.Name,
                    Amount = bfFermentable.Amount,
                    Type = bfFermentable.Type,
                    Supplier = bfFermentable.Supplier,
                    Origin = bfFermentable.Origin,
                    Color = bfFermentable.Color,
                    Potential = bfFermentable.Potential
                });
            }
        }

        if (bfRecipe.Hops != null)
        {
            foreach (var bfHop in bfRecipe.Hops)
            {
                recipe.RecipeHops.Add(new RecipeHop
                {
                    Name = bfHop.Name,
                    Amount = bfHop.Amount,
                    Alpha = bfHop.Alpha,
                    Type = bfHop.Type,
                    Origin = bfHop.Origin,
                    Use = bfHop.Use,
                    Time = bfHop.Time
                });
            }
        }

        if (bfRecipe.Yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.Yeasts)
            {
                recipe.RecipeYeasts.Add(new RecipeYeast
                {
                    Name = bfYeast.Name,
                    Amount = bfYeast.Amount,
                    Laboratory = bfYeast.Laboratory,
                    Type = bfYeast.Type,
                    Form = bfYeast.Form,
                    Attenuation = bfYeast.Attenuation,
                    Unit = bfYeast.Unit
                });
            }
        }

        if (bfRecipe.Miscs != null)
        {
            foreach (var bfMisc in bfRecipe.Miscs)
            {
                recipe.RecipeMiscs.Add(new RecipeMisc
                {
                    Name = bfMisc.Name,
                    Amount = bfMisc.Amount,
                    Type = bfMisc.Type,
                    Unit = bfMisc.Unit,
                    Use = bfMisc.Use,
                    Time = bfMisc.Time
                });
            }
        }

        return recipe;
    }

    public static void UpdateEntity(Recipe existingRecipe, BrewfatherRecipe bfRecipe)
    {
        existingRecipe.Name = bfRecipe.Name;
        existingRecipe.Style = bfRecipe.Style?.Name;

        existingRecipe.RecipeFermentables.Clear();
        existingRecipe.RecipeHops.Clear();
        existingRecipe.RecipeYeasts.Clear();
        existingRecipe.RecipeMiscs.Clear();

        if (bfRecipe.Fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.Fermentables)
            {
                existingRecipe.RecipeFermentables.Add(new RecipeFermentable
                {
                    Name = bfFermentable.Name,
                    Amount = bfFermentable.Amount,
                    Type = bfFermentable.Type,
                    Supplier = bfFermentable.Supplier,
                    Origin = bfFermentable.Origin,
                    Color = bfFermentable.Color,
                    Potential = bfFermentable.Potential
                });
            }
        }

        if (bfRecipe.Hops != null)
        {
            foreach (var bfHop in bfRecipe.Hops)
            {
                existingRecipe.RecipeHops.Add(new RecipeHop
                {
                    Name = bfHop.Name,
                    Amount = bfHop.Amount,
                    Alpha = bfHop.Alpha,
                    Type = bfHop.Type,
                    Origin = bfHop.Origin,
                    Use = bfHop.Use,
                    Time = bfHop.Time
                });
            }
        }

        if (bfRecipe.Yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.Yeasts)
            {
                existingRecipe.RecipeYeasts.Add(new RecipeYeast
                {
                    Name = bfYeast.Name,
                    Amount = bfYeast.Amount,
                    Laboratory = bfYeast.Laboratory,
                    Type = bfYeast.Type,
                    Form = bfYeast.Form,
                    Attenuation = bfYeast.Attenuation,
                    Unit = bfYeast.Unit
                });
            }
        }

        if (bfRecipe.Miscs != null)
        {
            foreach (var bfMisc in bfRecipe.Miscs)
            {
                existingRecipe.RecipeMiscs.Add(new RecipeMisc
                {
                    Name = bfMisc.Name,
                    Amount = bfMisc.Amount,
                    Type = bfMisc.Type,
                    Unit = bfMisc.Unit,
                    Use = bfMisc.Use,
                    Time = bfMisc.Time
                });
            }
        }
    }

    public static BrewfatherCreateRecipeRequest ToBrewfatherRequest(Recipe recipe)
    {
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
                Potential = rf.Potential
            }).ToList(),
            recipe.RecipeHops.Select(rh => new BrewfatherRecipeHop
            {
                Amount = rh.Amount,
                Name = rh.Name,
                Alpha = rh.Alpha,
                Type = rh.Type,
                Origin = rh.Origin,
                Use = rh.Use,
                Time = rh.Time
            }).ToList(),
            recipe.RecipeMiscs.Select(rm => new BrewfatherRecipeMisc
            {
                Amount = rm.Amount,
                Name = rm.Name,
                Type = rm.Type,
                Unit = rm.Unit,
                Use = rm.Use,
                Time = rm.Time
            }).ToList(),
            recipe.RecipeYeasts.Select(ry => new BrewfatherRecipeYeast
            {
                Amount = ry.Amount,
                Name = ry.Name,
                Laboratory = ry.Laboratory,
                Type = ry.Type,
                Form = ry.Form,
                Attenuation = ry.Attenuation,
                Unit = ry.Unit
            }).ToList()
        );
    }
}
