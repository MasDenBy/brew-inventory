using BrewInventory.App.Data;
using BrewInventory.App.Data.Entities;
using BrewInventory.App.Models;
using BrewInventory.App.Models.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Endpoints;

public static class FermentableEndpoints
{
    public static void MapFermentableEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/fermentables");

        group.MapGet("/", async (BrewInventoryContext db) =>
        {
            var items = await db.Fermentables
                .Select(f => new FermentableResponse(f.Id, f.Name, f.Amount, f.BestBefore, f.BrewfatherId, f.Supplier, f.Origin, f.Type.ToString(), f.Color))
                .ToListAsync();
            return Results.Ok(items);
        });

        group.MapGet("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var f = await db.Fermentables.FindAsync(id);
            return f is null
                ? Results.NotFound()
                : Results.Ok(new FermentableResponse(f.Id, f.Name, f.Amount, f.BestBefore, f.BrewfatherId, f.Supplier, f.Origin, f.Type.ToString(), f.Color));
        });

        group.MapPost("/", async (CreateFermentableRequest req, BrewInventoryContext db) =>
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
            // Try parse enum if available
            if (Enum.TryParse(typeof(FermentableType), req.Type, true, out var ft)) entity.Type = (FermentableType)ft;

            db.Fermentables.Add(entity);
            await db.SaveChangesAsync();

            var resp = new FermentableResponse(entity.Id, entity.Name, entity.Amount, entity.BestBefore, entity.BrewfatherId, entity.Supplier, entity.Origin, entity.Type.ToString(), entity.Color);
            return Results.Created($"/api/fermentables/{entity.Id}", resp);
        });

        group.MapPut("/{id:int}", async (int id, UpdateFermentableRequest req, BrewInventoryContext db) =>
        {
            var entity = await db.Fermentables.FindAsync(id);
            if (entity is null) return Results.NotFound();

            entity.Name = req.Name;
            entity.Amount = req.Amount;
            entity.BestBefore = req.BestBefore;
            entity.BrewfatherId = req.BrewfatherId;
            entity.Supplier = req.Supplier;
            entity.Origin = req.Origin;
            entity.Color = req.Color;
            if (Enum.TryParse(typeof(FermentableType), req.Type, true, out var ft)) entity.Type = (FermentableType)ft;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, BrewInventoryContext db) =>
        {
            var entity = await db.Fermentables.FindAsync(id);
            if (entity is null) return Results.NotFound();

            db.Fermentables.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}
