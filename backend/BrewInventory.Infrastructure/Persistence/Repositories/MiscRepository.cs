using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.Infrastructure.Persistence.Repositories;

public class MiscRepository : IMiscRepository
{
    private readonly BrewInventoryContext _context;

    public MiscRepository(BrewInventoryContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Misc>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Miscs.ToListAsync(cancellationToken);
    }

    public async Task<Misc?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Miscs.FindAsync([id], cancellationToken);
    }

    public async Task<Misc> AddAsync(Misc misc, CancellationToken cancellationToken = default)
    {
        _context.Miscs.Add(misc);
        await _context.SaveChangesAsync(cancellationToken);
        return misc;
    }

    public async Task UpdateAsync(Misc misc, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Misc misc, CancellationToken cancellationToken = default)
    {
        _context.Miscs.Remove(misc);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Miscs.AnyAsync(m => m.Id == id, cancellationToken);
    }
}
