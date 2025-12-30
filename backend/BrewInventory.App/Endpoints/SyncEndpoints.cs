using BrewInventory.App.Services;

namespace BrewInventory.App.Endpoints;

public static class SyncEndpoints
{
    public static void MapSyncEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/sync");

        group.MapPost("/fermentables", async (
            IBrewfatherSyncService syncService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await syncService.SyncFermentablesAsync(cancellationToken);
                return Results.Ok(new { message = "Fermentables synchronized successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Synchronization failed");
            }
        });

        group.MapPost("/hops", async (
            IBrewfatherSyncService syncService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await syncService.SyncHopsAsync(cancellationToken);
                return Results.Ok(new { message = "Hops synchronized successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Synchronization failed");
            }
        });

        group.MapPost("/yeasts", async (
            IBrewfatherSyncService syncService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await syncService.SyncYeastsAsync(cancellationToken);
                return Results.Ok(new { message = "Yeasts synchronized successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Synchronization failed");
            }
        });

        group.MapPost("/miscs", async (
            IBrewfatherSyncService syncService,
            CancellationToken cancellationToken) =>
        {
            try
            {
                await syncService.SyncMiscsAsync(cancellationToken);
                return Results.Ok(new { message = "Miscs synchronized successfully" });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Synchronization failed");
            }
        });
    }
}
