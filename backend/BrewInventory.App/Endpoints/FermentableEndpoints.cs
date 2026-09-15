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

        group.MapGet("/", async (IFermentableRepository repo, CancellationToken ct) =>
        {
            var items = await repo.GetAllAsync(ct);
            var responses = items.Select(f => new FermentableResponse(
                f.Id, f.Name, f.Amount, f.BestBefore, f.BrewfatherId,
                f.Supplier, f.Origin, f.Type.ToString(), f.Color)).ToList();
            return Results.Ok(responses);
        });

        group.MapGet("/{id:int}", async (int id, IFermentableRepository repo, CancellationToken ct) =>
        {
            var f = await repo.GetByIdAsync(id, ct);
            return f is null
                ? Results.NotFound()
                : Results.Ok(new FermentableResponse(
                    f.Id, f.Name, f.Amount, f.BestBefore, f.BrewfatherId,
                    f.Supplier, f.Origin, f.Type.ToString(), f.Color));
        });

        group.MapPost("/", async (CreateFermentableRequest req, IFermentableRepository repo, CancellationToken ct) =>
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

            await repo.AddAsync(entity, ct);

            var resp = new FermentableResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId, entity.Supplier, entity.Origin, entity.Type.ToString(), entity.Color);
            return Results.Created($"/api/fermentables/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateFermentableRequest req, IFermentableRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;
            entity.Supplier = req.Supplier;
            entity.Origin = req.Origin;
            entity.Color = req.Color;
            if (Enum.TryParse(typeof(FermentableType), req.Type, true, out var ft)) entity.Type = (FermentableType)ft;

            await repo.UpdateAsync(entity, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IFermentableRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            await repo.DeleteAsync(entity, ct);
            return Results.NoContent();
        });
    }
}
