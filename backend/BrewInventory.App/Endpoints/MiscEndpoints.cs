using BrewInventory.App.Data;
using BrewInventory.App.Data.Entities;
using BrewInventory.App.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Endpoints;

public static class MiscEndpoints
{
    public static void MapMiscEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/miscs");

        group.MapGet("/", async (BrewInventoryContext db) =>
        {
            var items = await db.Miscs
                .Select(m => new MiscResponse(m.Id, m.Name, m.Amount, m.BestBefore, m.BrewfatherId))
                .ToListAsync();
            return Results.Ok(items);
        });

        group.MapGet("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var m = await db.Miscs.FindAsync(id);
            return m is null
                ? Results.NotFound()
                : Results.Ok(new MiscResponse(m.Id, m.Name, m.Amount, m.BestBefore, m.BrewfatherId));
        });

        group.MapPost("/", async (CreateMiscRequest req, BrewInventoryContext db) =>
        {
            var entity = new Misc
            {
                Name = req.Name,
                Amount = req.Amount,
                BestBefore = req.BestBefore,
                BrewfatherId = req.BrewfatherId
            };

            db.Miscs.Add(entity);
            await db.SaveChangesAsync();

            var resp = new MiscResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId);
            return Results.Created($"/api/miscs/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateMiscRequest req, BrewInventoryContext db) =>
        {
            var entity = await db.Miscs.FindAsync(id);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var entity = await db.Miscs.FindAsync(id);
            if (entity is null) return Results.NotFound();

            db.Miscs.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
