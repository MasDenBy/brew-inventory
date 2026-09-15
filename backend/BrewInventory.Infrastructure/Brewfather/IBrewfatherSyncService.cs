using BrewInventory.Domain.Entities;

namespace BrewInventory.Infrastructure.Brewfather;

public interface IBrewfatherSyncService
{
    Task SyncFermentablesAsync(CancellationToken cancellationToken = default);
    Task SyncHopsAsync(CancellationToken cancellationToken = default);
    Task SyncYeastsAsync(CancellationToken cancellationToken = default);
    Task SyncMiscsAsync(CancellationToken cancellationToken = default);
    Task SyncRecipesAsync(CancellationToken cancellationToken = default);
    Task<Recipe> PushRecipeToBrewfatherAsync(int recipeId, CancellationToken cancellationToken = default);
}
