using BrewInventory.App.Data;
using BrewInventory.App.Data.Entities;
using BrewInventory.App.Models;
using BrewInventory.App.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Endpoints;

public static class YeastEndpoints
{
    public static void MapYeastEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/yeasts");

        group.MapGet("/", async (BrewInventoryContext db) =>
        {
            var items = await db.Yeasts
                .Select(y => new YeastResponse(y.Id, y.Name, y.Amount, y.BestBefore, y.BrewfatherId, y.Labaratory, y.Type.ToString(), y.Form.ToString()))
                .ToListAsync();
            return Results.Ok(items);
        });

        group.MapGet("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var y = await db.Yeasts.FindAsync(id);
            return y is null
                ? Results.NotFound()
                : Results.Ok(new YeastResponse(y.Id, y.Name, y.Amount, y.BestBefore, y.BrewfatherId, y.Labaratory, y.Type.ToString(), y.Form.ToString()));
        });

        group.MapPost("/", async (CreateYeastRequest req, BrewInventoryContext db) =>
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

            db.Yeasts.Add(entity);
            await db.SaveChangesAsync();

            var resp = new YeastResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId, entity.Labaratory, entity.Type.ToString(), entity.Form.ToString());
            return Results.Created($"/api/yeasts/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateYeastRequest req, BrewInventoryContext db) =>
        {
            var entity = await db.Yeasts.FindAsync(id);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;
            entity.Labaratory = req.Laboratory;
            if (Enum.TryParse(typeof(YeastType), req.Type, true, out var yt)) entity.Type = (YeastType)yt;
            if (Enum.TryParse(typeof(YeastForm), req.Form, true, out var yf)) entity.Form = (YeastForm)yf;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var entity = await db.Yeasts.FindAsync(id);
            if (entity is null) return Results.NotFound();

            db.Yeasts.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
