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

        group.MapGet("/", async (IMiscRepository repo, CancellationToken ct) =>
        {
            var items = await repo.GetAllAsync(ct);
            var responses = items.Select(m => new MiscResponse(
                m.Id, m.Name, m.Amount, m.Unit, m.Type, m.BrewfatherId)).ToList();
            return Results.Ok(responses);
        });

        group.MapGet("/{id:int}", async (int id, IMiscRepository repo, CancellationToken ct) =>
        {
            var m = await repo.GetByIdAsync(id, ct);
            return m is null
                ? Results.NotFound()
                : Results.Ok(new MiscResponse(m.Id, m.Name, m.Amount, m.Unit, m.Type, m.BrewfatherId));
        });

        group.MapPost("/", async (CreateMiscRequest req, IMiscRepository repo, CancellationToken ct) =>
        {
            var entity = new Misc
            {
                Name = req.Name,
                Amount = req.Amount,
                Unit = req.Unit,
                Type = req.Type,
                BrewfatherId = req.BrewfatherId
            };

            await repo.AddAsync(entity, ct);

            var resp = new MiscResponse(entity.Id, entity.Name, entity.Amount, entity.Unit, entity.Type, entity.BrewfatherId);
            return Results.Created($"/api/miscs/{entity.Id.ToString(CultureInfo.InvariantCulture)}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateMiscRequest req, IMiscRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.Unit = req.Unit;
            entity.Type = req.Type;
            entity.BrewfatherId = req.BrewfatherId;

            await repo.UpdateAsync(entity, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IMiscRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            await repo.DeleteAsync(entity, ct);
            return Results.NoContent();
        });
    }
}
