using BrewInventory.App.Data;
using BrewInventory.App.Data.Entities;
using BrewInventory.App.Models;
using BrewInventory.App.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Endpoints;

public static class HopEndpoints
{
    public static void MapHopEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/hops");

        group.MapGet("/", async (BrewInventoryContext db) =>
        {
            var items = await db.Hops
                .Select(h => new HopResponse(h.Id, h.Name, h.Amount, h.BestBefore, h.BrewfatherId, h.Origin, h.Type.ToString(), h.AlphaAcid, h.HarvestYear))
                .ToListAsync();
            return Results.Ok(items);
        });

        group.MapGet("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var h = await db.Hops.FindAsync(id);
            return h is null
                ? Results.NotFound()
                : Results.Ok(new HopResponse(h.Id, h.Name, h.Amount, h.BestBefore, h.BrewfatherId, h.Origin, h.Type.ToString(), h.AlphaAcid, h.HarvestYear));
        });

        group.MapPost("/", async (CreateHopRequest req, BrewInventoryContext db) =>
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

            db.Hops.Add(entity);
            await db.SaveChangesAsync();

            var resp = new HopResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId, entity.Origin, entity.Type.ToString(), entity.AlphaAcid, entity.HarvestYear);
            return Results.Created($"/api/hops/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateHopRequest req, BrewInventoryContext db) =>
        {
            var entity = await db.Hops.FindAsync(id);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;
            entity.Origin = req.Origin;
            entity.AlphaAcid = req.AlphaAcid;
            entity.HarvestYear = req.HarvestYear;
            if (Enum.TryParse(typeof(HopType), req.Type, true, out var ht)) entity.Type = (HopType)ht;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var entity = await db.Hops.FindAsync(id);
            if (entity is null) return Results.NotFound();

            db.Hops.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
