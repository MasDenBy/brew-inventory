using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.Infrastructure.Persistence.Repositories;

public class FermentableRepository : IFermentableRepository
{
    private readonly BrewInventoryContext _context;

    public FermentableRepository(BrewInventoryContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Fermentable>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Fermentables.ToListAsync(cancellationToken);
    }

    public async Task<Fermentable?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Fermentables.FindAsync([id], cancellationToken);
    }

    public async Task<Fermentable> AddAsync(Fermentable fermentable, CancellationToken cancellationToken = default)
    {
        _context.Fermentables.Add(fermentable);
        await _context.SaveChangesAsync(cancellationToken);
        return fermentable;
    }

    public async Task UpdateAsync(Fermentable fermentable, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Fermentable fermentable, CancellationToken cancellationToken = default)
    {
        _context.Fermentables.Remove(fermentable);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Fermentables.AnyAsync(f => f.Id == id, cancellationToken);
    }
}
