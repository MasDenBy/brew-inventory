using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Repositories;

public interface IRecipeRepository
{
    Task<ICollection<Recipe>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ICollection<Recipe>> GetByIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default);
    Task<Recipe> AddAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task DeleteAsync(Recipe recipe, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
