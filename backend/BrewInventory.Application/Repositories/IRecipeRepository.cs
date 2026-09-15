using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Repositories;

public interface IRecipeRepository
{
    Task<List<Recipe>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Recipe>> GetByIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
    Task<Recipe> AddAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task DeleteAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<List<int>> GetExistingFermentableIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
    Task<List<int>> GetExistingHopIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
    Task<List<int>> GetExistingYeastIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
    Task<List<int>> GetExistingMiscIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
}
