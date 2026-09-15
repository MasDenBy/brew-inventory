using BrewInventory.Infrastructure.Brewfather.Models;

namespace BrewInventory.Infrastructure.Brewfather;

public interface IBrewfatherClient
{
    Task<List<T>> GetAllInventoryItemsAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllItemsAsync<T>(string endpoint, string? include = null, CancellationToken cancellationToken = default);
    Task<string> CreateRecipeAsync(BrewfatherCreateRecipeRequest recipe, CancellationToken cancellationToken = default);
}
