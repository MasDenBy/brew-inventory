using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Repositories;

public interface IFermentableRepository
{
    Task<List<Fermentable>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Fermentable?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Fermentable> AddAsync(Fermentable fermentable, CancellationToken cancellationToken = default);
    Task UpdateAsync(Fermentable fermentable, CancellationToken cancellationToken = default);
    Task DeleteAsync(Fermentable fermentable, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
