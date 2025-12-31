using BrewInventory.App.Data;
using BrewInventory.App.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Endpoints;

public static class RecipeEndpoints
{
    public static void MapRecipeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/recipes");

        group.MapGet("/", async (BrewInventoryContext db) =>
        {
            var recipes = await db.Recipes
                .Select(r => new RecipeListResponse(
                    r.Id,
                    r.Name,
                    r.BrewfatherId))
                .ToListAsync();

            return Results.Ok(recipes);
        });

        group.MapGet("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var recipe = await db.Recipes
                .Include(r => r.RecipeFermentables)
                    .ThenInclude(rf => rf.Fermentable)
                .Include(r => r.RecipeHops)
                    .ThenInclude(rh => rh.Hop)
                .Include(r => r.RecipeYeasts)
                    .ThenInclude(ry => ry.Yeast)
                .Include(r => r.RecipeMiscs)
                    .ThenInclude(rm => rm.Misc)
                .Where(r => r.Id == id)
                .Select(r => new RecipeDetailsResponse(
                    r.Id,
                    r.Name,
                    r.BrewfatherId,
                    r.RecipeFermentables.Select(rf => new RecipeFermentableDetail(
                        rf.Fermentable.Id,
                        rf.Fermentable.Name,
                        rf.Fermentable.Type.ToString(),
                        rf.Amount,
                        rf.Fermentable.Supplier,
                        rf.Fermentable.Origin,
                        rf.Fermentable.Color
                    )).ToList(),
                    r.RecipeHops.Select(rh => new RecipeHopDetail(
                        rh.Hop.Id,
                        rh.Hop.Name,
                        rh.Hop.Type.ToString(),
                        rh.Amount,
                        rh.Hop.Origin,
                        rh.Hop.AlphaAcid,
                        rh.Hop.HarvestYear
                    )).ToList(),
                    r.RecipeYeasts.Select(ry => new RecipeYeastDetail(
                        ry.Yeast.Id,
                        ry.Yeast.Name,
                        ry.Yeast.Type.ToString(),
                        ry.Yeast.Form.ToString(),
                        ry.Amount,
                        ry.Yeast.Labaratory
                    )).ToList(),
                    r.RecipeMiscs.Select(rm => new RecipeMiscDetail(
                        rm.Misc.Id,
                        rm.Misc.Name,
                        rm.Misc.Type.ToString(),
                        rm.Misc.Unit.ToString(),
                        rm.Amount
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            return recipe is null
                ? Results.NotFound()
                : Results.Ok(recipe);
        });
    }
}
