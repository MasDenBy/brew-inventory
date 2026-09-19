using BrewInventory.Application.Contracts.Recipes;
using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using BrewInventory.Infrastructure.Brewfather;

namespace BrewInventory.App.Endpoints;

internal static class RecipeEndpoints
{
    public static void MapRecipeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/recipes");

        group.MapGet("/", async (IRecipeRepository repo, CancellationToken ct) =>
        {
            var recipes = await repo.GetAllAsync(ct);
            var responses = recipes.Select(r => new RecipeListResponse(r.Id, r.Name, r.BrewfatherId)).ToList();
            return Results.Ok(responses);
        });

        group.MapGet("/{id:int}", async (int id, IRecipeRepository repo, CancellationToken ct) =>
        {
            var recipe = await repo.GetByIdAsync(id, ct);
            if (recipe is null) return Results.NotFound();

            var response = new RecipeDetailsResponse(
                recipe.Id,
                recipe.Name,
                recipe.BrewfatherId,
                recipe.RecipeFermentables.Select(rf => new RecipeFermentableDetail(
                    rf.Fermentable.Id,
                    rf.Fermentable.Name,
                    rf.Fermentable.Type.ToString(),
                    rf.Amount,
                    rf.Fermentable.Supplier,
                    rf.Fermentable.Origin,
                    rf.Fermentable.Color
                )).ToList(),
                recipe.RecipeHops.Select(rh => new RecipeHopDetail(
                    rh.Hop.Id,
                    rh.Hop.Name,
                    rh.Hop.Type.ToString(),
                    rh.Amount,
                    rh.Hop.Origin,
                    rh.Hop.AlphaAcid,
                    rh.Hop.HarvestYear
                )).ToList(),
                recipe.RecipeYeasts.Select(ry => new RecipeYeastDetail(
                    ry.Yeast.Id,
                    ry.Yeast.Name,
                    ry.Yeast.Type.ToString(),
                    ry.Yeast.Form.ToString(),
                    ry.Amount,
                    ry.Yeast.Laboratory
                )).ToList(),
                recipe.RecipeMiscs.Select(rm => new RecipeMiscDetail(
                    rm.Misc.Id,
                    rm.Misc.Name,
                    rm.Misc.Type.ToString(),
                    rm.Misc.Unit.ToString(),
                    rm.Amount
                )).ToList()
            );

            return Results.Ok(response);
        });

        group.MapPost("/", async (CreateRecipeRequest req, IRecipeRepository repo, CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(req.Name))
            {
                return Results.BadRequest(new { message = "Recipe name is required." });
            }

            var fermentables = req.Fermentables ?? [];
            var hops = req.Hops ?? [];
            var yeasts = req.Yeasts ?? [];
            var miscs = req.Miscs ?? [];

            var missingIds = await FindMissingIngredientIdsAsync(repo, fermentables, hops, yeasts, miscs, ct);
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

            await repo.AddAsync(recipe, ct);

            var details = await repo.GetByIdAsync(recipe.Id, ct);
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

    private static async Task<List<string>> FindMissingIngredientIdsAsync(
        IRecipeRepository repo,
        ICollection<CreateRecipeFermentableRequest> fermentables,
        ICollection<CreateRecipeHopRequest> hops,
        ICollection<CreateRecipeYeastRequest> yeasts,
        ICollection<CreateRecipeMiscRequest> miscs,
        CancellationToken ct)
    {
        var missing = new List<string>();

        var fermentableIds = fermentables.Select(f => f.FermentableId).Distinct().ToList();
        if (fermentableIds.Count > 0)
        {
            var foundIds = await repo.GetExistingFermentableIdsAsync(fermentableIds, ct);
            missing.AddRange(fermentableIds.Except(foundIds).Select(id => $"Fermentable {id}"));
        }

        var hopIds = hops.Select(h => h.HopId).Distinct().ToList();
        if (hopIds.Count > 0)
        {
            var foundIds = await repo.GetExistingHopIdsAsync(hopIds, ct);
            missing.AddRange(hopIds.Except(foundIds).Select(id => $"Hop {id}"));
        }

        var yeastIds = yeasts.Select(y => y.YeastId).Distinct().ToList();
        if (yeastIds.Count > 0)
        {
            var foundIds = await repo.GetExistingYeastIdsAsync(yeastIds, ct);
            missing.AddRange(yeastIds.Except(foundIds).Select(id => $"Yeast {id}"));
        }

        var miscIds = miscs.Select(m => m.MiscId).Distinct().ToList();
        if (miscIds.Count > 0)
        {
            var foundIds = await repo.GetExistingMiscIdsAsync(miscIds, ct);
            missing.AddRange(miscIds.Except(foundIds).Select(id => $"Misc {id}"));
        }

        return missing;
    }
}
