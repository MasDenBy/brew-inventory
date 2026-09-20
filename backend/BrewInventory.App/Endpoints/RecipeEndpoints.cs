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
                recipe.Style,
                recipe.RecipeFermentables.Select(rf => new RecipeFermentableDetail(
                    rf.Name,
                    rf.Amount,
                    rf.Type.ToString(),
                    rf.Supplier,
                    rf.Origin,
                    rf.Color,
                    rf.Potential
                )).ToList(),
                recipe.RecipeHops.Select(rh => new RecipeHopDetail(
                    rh.Name,
                    rh.Amount,
                    rh.Alpha,
                    rh.Type.ToString(),
                    rh.Origin,
                    rh.Use.ToString(),
                    rh.Time
                )).ToList(),
                recipe.RecipeYeasts.Select(ry => new RecipeYeastDetail(
                    ry.Name,
                    ry.Amount,
                    ry.Laboratory,
                    ry.Type.ToString(),
                    ry.Form.ToString(),
                    ry.Attenuation,
                    ry.Unit
                )).ToList(),
                recipe.RecipeMiscs.Select(rm => new RecipeMiscDetail(
                    rm.Name,
                    rm.Amount,
                    rm.Type.ToString(),
                    rm.Unit,
                    rm.Use,
                    rm.Time
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

            var recipe = new Recipe
            {
                Name = req.Name,
                Style = req.Style
            };

            foreach (var f in req.Fermentables ?? [])
            {
                recipe.RecipeFermentables.Add(new RecipeFermentable
                {
                    Name = f.Name,
                    Amount = f.Amount,
                    Type = f.Type,
                    Supplier = f.Supplier,
                    Origin = f.Origin,
                    Color = f.Color,
                    Potential = f.Potential
                });
            }

            foreach (var h in req.Hops ?? [])
            {
                recipe.RecipeHops.Add(new RecipeHop
                {
                    Name = h.Name,
                    Amount = h.Amount,
                    Alpha = h.Alpha,
                    Type = h.Type,
                    Origin = h.Origin,
                    Use = h.Use,
                    Time = h.Time
                });
            }

            foreach (var y in req.Yeasts ?? [])
            {
                recipe.RecipeYeasts.Add(new RecipeYeast
                {
                    Name = y.Name,
                    Amount = y.Amount,
                    Laboratory = y.Laboratory,
                    Type = y.Type,
                    Form = y.Form,
                    Attenuation = y.Attenuation,
                    Unit = y.Unit
                });
            }

            foreach (var m in req.Miscs ?? [])
            {
                recipe.RecipeMiscs.Add(new RecipeMisc
                {
                    Name = m.Name,
                    Amount = m.Amount,
                    Type = m.Type,
                    Unit = m.Unit,
                    Use = m.Use,
                    Time = m.Time
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
}
