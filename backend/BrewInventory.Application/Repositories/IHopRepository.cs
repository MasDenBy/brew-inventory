using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Repositories;

public interface IHopRepository
{
    Task<List<Hop>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Hop?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Hop> AddAsync(Hop hop, CancellationToken cancellationToken = default);
    Task UpdateAsync(Hop hop, CancellationToken cancellationToken = default);
    Task DeleteAsync(Hop hop, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
