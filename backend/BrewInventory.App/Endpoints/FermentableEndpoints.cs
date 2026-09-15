using BrewInventory.Application.Contracts.Fermentables;
using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Application.Repositories;

namespace BrewInventory.App.Endpoints;

public static class FermentableEndpoints
{
    public static void MapFermentableEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/fermentables");

        group.MapGet("/", async (IIngredientRepository repo, CancellationToken ct) =>
        {
            var items = await repo.GetAllFermentablesAsync(ct);
            var responses = items.Select(f => new FermentableResponse(
                f.Id, f.Name, f.Amount, f.BestBefore, f.BrewfatherId,
                f.Supplier, f.Origin, f.Type.ToString(), f.Color)).ToList();
            return Results.Ok(responses);
        });

        group.MapGet("/{id:int}", async (int id, IIngredientRepository repo, CancellationToken ct) =>
        {
            var f = await repo.GetFermentableByIdAsync(id, ct);
            return f is null
                ? Results.NotFound()
                : Results.Ok(new FermentableResponse(
                    f.Id, f.Name, f.Amount, f.BestBefore, f.BrewfatherId,
                    f.Supplier, f.Origin, f.Type.ToString(), f.Color));
        });

        group.MapPost("/", async (CreateFermentableRequest req, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = new Fermentable
            {
                Name = req.Name,
                Amount = req.Amount,
                BestBefore = req.BestBefore,
                BrewfatherId = req.BrewfatherId,
                Supplier = req.Supplier,
                Origin = req.Origin,
                Color = req.Color
            };
            if (Enum.TryParse(typeof(FermentableType), req.Type, true, out var ft)) entity.Type = (FermentableType)ft;

            await repo.AddFermentableAsync(entity, ct);

            var resp = new FermentableResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId, entity.Supplier, entity.Origin, entity.Type.ToString(), entity.Color);
            return Results.Created($"/api/fermentables/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateFermentableRequest req, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetFermentableByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;
            entity.Supplier = req.Supplier;
            entity.Origin = req.Origin;
            entity.Color = req.Color;
            if (Enum.TryParse(typeof(FermentableType), req.Type, true, out var ft)) entity.Type = (FermentableType)ft;

            await repo.UpdateFermentableAsync(entity, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetFermentableByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            await repo.DeleteFermentableAsync(entity, ct);
            return Results.NoContent();
        });
    }
}
