using BrewInventory.App.Data;
using BrewInventory.App.Data.Entities;
using BrewInventory.App.Models.Contracts;
using BrewInventory.App.Services;
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

        group.MapGet("/{id:int}", async (int id, BrewInventoryContext db, CancellationToken ct) =>
        {
            var recipe = await LoadRecipeDetails(db, id, ct);

            return recipe is null
                ? Results.NotFound()
                : Results.Ok(recipe);
        });

        group.MapPost("/", async (CreateRecipeRequest req, BrewInventoryContext db, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(req.Name))
            {
                return Results.BadRequest(new { message = "Recipe name is required." });
            }

            var fermentables = req.Fermentables ?? [];
            var hops = req.Hops ?? [];
            var yeasts = req.Yeasts ?? [];
            var miscs = req.Miscs ?? [];

            var missingIds = await FindMissingIngredientIds(
                db, fermentables, hops, yeasts, miscs, ct);
            if (missingIds.Count > 0)
            {
                return Results.BadRequest(new
                {
                    message = "One or more referenced ingredients do not exist.",
                    missingIds
                });
            }

            var recipe = new Recipe { Name = req.Name };

            foreach (var f in fermentables)
            {
                recipe.RecipeFermentables.Add(new RecipeFermentable
                {
                    FermentableId = f.FermentableId,
                    Amount = f.Amount
                });
            }

            foreach (var h in hops)
            {
                recipe.RecipeHops.Add(new RecipeHop
                {
                    HopId = h.HopId,
                    Amount = h.Amount
                });
            }

            foreach (var y in yeasts)
            {
                recipe.RecipeYeasts.Add(new RecipeYeast
                {
                    YeastId = y.YeastId,
                    Amount = y.Amount
                });
            }

            foreach (var m in miscs)
            {
                recipe.RecipeMiscs.Add(new RecipeMisc
                {
                    MiscId = m.MiscId,
                    Amount = m.Amount
                });
            }

            db.Recipes.Add(recipe);
            await db.SaveChangesAsync(ct);

            var details = await LoadRecipeDetails(db, recipe.Id, ct);
            return Results.Created($"/api/recipes/{recipe.Id}", details);
        });

        group.MapPost("/{id:int}/sync", async (int id, IBrewfatherSyncService syncService, CancellationToken ct) =>
        {
            try
            {
                var recipe = await syncService.PushRecipeToBrewfatherAsync(id, ct);
                return Results.Ok(new SyncRecipeResponse(recipe.Id, recipe.Name, recipe.BrewfatherId!));
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { message = ex.Message });
            }
        });
    }

    private static async Task<List<string>> FindMissingIngredientIds(
        BrewInventoryContext db,
        List<CreateRecipeFermentableRequest> fermentables,
        List<CreateRecipeHopRequest> hops,
        List<CreateRecipeYeastRequest> yeasts,
        List<CreateRecipeMiscRequest> miscs,
        CancellationToken ct)
    {
        var missing = new List<string>();

        var fermentableIds = fermentables.Select(f => f.FermentableId).Distinct().ToList();
        if (fermentableIds.Count > 0)
        {
            var foundIds = await db.Fermentables
                .Where(f => fermentableIds.Contains(f.Id))
                .Select(f => f.Id)
                .ToListAsync(ct);
            missing.AddRange(fermentableIds.Except(foundIds).Select(id => $"Fermentable {id}"));
        }

        var hopIds = hops.Select(h => h.HopId).Distinct().ToList();
        if (hopIds.Count > 0)
        {
            var foundIds = await db.Hops
                .Where(h => hopIds.Contains(h.Id))
                .Select(h => h.Id)
                .ToListAsync(ct);
            missing.AddRange(hopIds.Except(foundIds).Select(id => $"Hop {id}"));
        }

        var yeastIds = yeasts.Select(y => y.YeastId).Distinct().ToList();
        if (yeastIds.Count > 0)
        {
            var foundIds = await db.Yeasts
                .Where(y => yeastIds.Contains(y.Id))
                .Select(y => y.Id)
                .ToListAsync(ct);
            missing.AddRange(yeastIds.Except(foundIds).Select(id => $"Yeast {id}"));
        }

        var miscIds = miscs.Select(m => m.MiscId).Distinct().ToList();
        if (miscIds.Count > 0)
        {
            var foundIds = await db.Miscs
                .Where(m => miscIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync(ct);
            missing.AddRange(miscIds.Except(foundIds).Select(id => $"Misc {id}"));
        }

        return missing;
    }

    private static async Task<RecipeDetailsResponse?> LoadRecipeDetails(BrewInventoryContext db, int id, CancellationToken ct = default)
    {
        return await db.Recipes
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
            .FirstOrDefaultAsync(ct);
    }
}