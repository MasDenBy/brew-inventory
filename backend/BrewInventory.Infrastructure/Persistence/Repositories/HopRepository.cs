using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.Infrastructure.Persistence.Repositories;

public class HopRepository : IHopRepository
{
    private readonly BrewInventoryContext _context;

    public HopRepository(BrewInventoryContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Hop>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Hops.ToListAsync(cancellationToken);
    }

    public async Task<Hop?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Hops.FindAsync([id], cancellationToken);
    }

    public async Task<Hop> AddAsync(Hop hop, CancellationToken cancellationToken = default)
    {
        _context.Hops.Add(hop);
        await _context.SaveChangesAsync(cancellationToken);
        return hop;
    }

    public async Task UpdateAsync(Hop hop, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Hop hop, CancellationToken cancellationToken = default)
    {
        _context.Hops.Remove(hop);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Hops.AnyAsync(h => h.Id == id, cancellationToken);
    }
}
