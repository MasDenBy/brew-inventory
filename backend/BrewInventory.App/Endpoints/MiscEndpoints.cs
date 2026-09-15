using System.Globalization;
using BrewInventory.Application.Contracts.Miscs;
using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;

namespace BrewInventory.App.Endpoints;

internal static class MiscEndpoints
{
    public static void MapMiscEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/miscs");

        group.MapGet("/", async (IIngredientRepository repo, CancellationToken ct) =>
        {
            var items = await repo.GetAllMiscsAsync(ct);
            var responses = items.Select(m => new MiscResponse(
                m.Id, m.Name, m.Amount, m.Unit, m.Type, m.BestBefore, m.BrewfatherId)).ToList();
            return Results.Ok(responses);
        });

        group.MapGet("/{id:int}", async (int id, IIngredientRepository repo, CancellationToken ct) =>
        {
            var m = await repo.GetMiscByIdAsync(id, ct);
            return m is null
                ? Results.NotFound()
                : Results.Ok(new MiscResponse(m.Id, m.Name, m.Amount, m.Unit, m.Type, m.BestBefore, m.BrewfatherId));
        });

        group.MapPost("/", async (CreateMiscRequest req, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = new Misc
            {
                Name = req.Name,
                Amount = req.Amount,
                Unit = req.Unit,
                Type = req.Type,
                BestBefore = req.BestBefore,
                BrewfatherId = req.BrewfatherId
            };

            await repo.AddMiscAsync(entity, ct);

            var resp = new MiscResponse(entity.Id, entity.Name, entity.Amount, entity.Unit, entity.Type, entity.BestBefore, entity.BrewfatherId);
            return Results.Created($"/api/miscs/{entity.Id.ToString(CultureInfo.InvariantCulture)}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateMiscRequest req, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetMiscByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.Unit = req.Unit;
            entity.Type = req.Type;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;

            await repo.UpdateMiscAsync(entity, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IIngredientRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetMiscByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            await repo.DeleteMiscAsync(entity, ct);
            return Results.NoContent();
        });
    }
}
