using BrewInventory.App.Endpoints;
using BrewInventory.Application.Contracts.Hops;
using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Application.Repositories;

namespace BrewInventory.App.Endpoints;

public static class HopEndpoints
{
    public static void MapHopEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/hops");

        group.MapGet("/", async (IIngredientRepository repo, CancellationToken ct) =>
        {
            var items = await repo.GetAllHopsAsync(ct);
            var responses = items.Select(h => new HopResponse(
                h.Id, h.Name, h.Amount, h.BestBefore, h.BrewfatherId,
                h.Origin, h.Type.ToString(), h.AlphaAcid, h.HarvestYear)).ToList();
            return Results.Ok(responses);
        });

        group.MapGet("/{id:int}", async (int id, IIngredientRepository repo, CancellationToken ct) =>
        {
            var h = await repo.GetHopByIdAsync(id, ct);
            return h is null
                ? Results.NotFound()
                : Results.Ok(new HopResponse(
                    h.Id, h.Name, h.Amount, h.BestBefore, h.BrewfatherId,
                    h.Origin, h.Type.ToString(), h.AlphaAcid, h.HarvestYear));
        });

        group.MapPost("/", async (CreateHopRequest req, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = new Hop
            {
                Name = req.Name,
                Amount = req.Amount,
                BestBefore = req.BestBefore,
                BrewfatherId = req.BrewfatherId,
                Origin = req.Origin,
                AlphaAcid = req.AlphaAcid,
                HarvestYear = req.HarvestYear
            };
            if (Enum.TryParse(typeof(HopType), req.Type, true, out var ht)) entity.Type = (HopType)ht;

            await repo.AddHopAsync(entity, ct);

            var resp = new HopResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId, entity.Origin, entity.Type.ToString(), entity.AlphaAcid, entity.HarvestYear);
            return Results.Created($"/api/hops/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateHopRequest req, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetHopByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;
            entity.Origin = req.Origin;
            entity.AlphaAcid = req.AlphaAcid;
            entity.HarvestYear = req.HarvestYear;
            if (Enum.TryParse(typeof(HopType), req.Type, true, out var ht)) entity.Type = (HopType)ht;

            await repo.UpdateHopAsync(entity, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetHopByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            await repo.DeleteHopAsync(entity, ct);
            return Results.NoContent();
        });
    }
}
