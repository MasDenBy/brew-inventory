using BrewInventory.Application.Contracts.Yeasts;
using BrewInventory.Domain.Entities;
using BrewInventory.Domain.Entities.Enums;
using BrewInventory.Application.Repositories;

namespace BrewInventory.App.Endpoints;

internal static class YeastEndpoints
{
    public static void MapYeastEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/yeasts");

        group.MapGet("/", async (IYeastRepository repo, CancellationToken ct) =>
        {
            var items = await repo.GetAllAsync(ct);
            var responses = items.Select(y => new YeastResponse(
                y.Id, y.Name, y.Amount, y.BestBefore, y.BrewfatherId,
                y.Labaratory, y.Type.ToString(), y.Form.ToString())).ToList();
            return Results.Ok(responses);
        });

        group.MapGet("/{id:int}", async (int id, IYeastRepository repo, CancellationToken ct) =>
        {
            var y = await repo.GetByIdAsync(id, ct);
            return y is null
                ? Results.NotFound()
                : Results.Ok(new YeastResponse(
                    y.Id, y.Name, y.Amount, y.BestBefore, y.BrewfatherId,
                    y.Labaratory, y.Type.ToString(), y.Form.ToString()));
        });

        group.MapPost("/", async (CreateYeastRequest req, IYeastRepository repo, CancellationToken ct) =>
        {
            var entity = new Yeast
            {
                Name = req.Name,
                Amount = req.Amount,
                BestBefore = req.BestBefore,
                BrewfatherId = req.BrewfatherId,
                Labaratory = req.Laboratory
            };
            if (Enum.TryParse(typeof(YeastType), req.Type, true, out var yt)) entity.Type = (YeastType)yt;
            if (Enum.TryParse(typeof(YeastForm), req.Form, true, out var yf)) entity.Form = (YeastForm)yf;

            await repo.AddAsync(entity, ct);

            var resp = new YeastResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId, entity.Labaratory, entity.Type.ToString(), entity.Form.ToString());
            return Results.Created($"/api/yeasts/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateYeastRequest req, IYeastRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;
            entity.Labaratory = req.Laboratory;
            if (Enum.TryParse(typeof(YeastType), req.Type, true, out var yt)) entity.Type = (YeastType)yt;
            if (Enum.TryParse(typeof(YeastForm), req.Form, true, out var yf)) entity.Form = (YeastForm)yf;

            await repo.UpdateAsync(entity, ct);
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, IYeastRepository repo, CancellationToken ct) =>
        {
            var entity = await repo.GetByIdAsync(id, ct);
            if (entity is null) return Results.NotFound();

            await repo.DeleteAsync(entity, ct);
            return Results.NoContent();
        });
    }
}
