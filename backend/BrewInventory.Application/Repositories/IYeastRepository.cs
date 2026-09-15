using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Repositories;

public interface IYeastRepository
{
    Task<List<Yeast>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Yeast?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Yeast> AddAsync(Yeast yeast, CancellationToken cancellationToken = default);
    Task UpdateAsync(Yeast yeast, CancellationToken cancellationToken = default);
    Task DeleteAsync(Yeast yeast, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
