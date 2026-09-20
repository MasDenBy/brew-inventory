using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Repositories;

public interface IMiscRepository
{
    Task<ICollection<Misc>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Misc?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Misc> AddAsync(Misc misc, CancellationToken cancellationToken = default);
    Task UpdateAsync(Misc misc, CancellationToken cancellationToken = default);
    Task DeleteAsync(Misc misc, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
