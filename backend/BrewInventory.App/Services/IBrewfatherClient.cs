namespace BrewInventory.App.Services;

public interface IBrewfatherClient
{
    Task<List<T>> GetAllInventoryItemsAsync<T>(string endpoint, CancellationToken cancellationToken = default);
}
