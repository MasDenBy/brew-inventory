using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.Infrastructure.Persistence.Repositories;

public class YeastRepository : IYeastRepository
{
    private readonly BrewInventoryContext _context;

    public YeastRepository(BrewInventoryContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Yeast>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Yeasts.ToListAsync(cancellationToken);
    }

    public async Task<Yeast?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Yeasts.FindAsync([id], cancellationToken);
    }

    public async Task<Yeast> AddAsync(Yeast yeast, CancellationToken cancellationToken = default)
    {
        _context.Yeasts.Add(yeast);
        await _context.SaveChangesAsync(cancellationToken);
        return yeast;
    }

    public async Task UpdateAsync(Yeast yeast, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Yeast yeast, CancellationToken cancellationToken = default)
    {
        _context.Yeasts.Remove(yeast);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Yeasts.AnyAsync(y => y.Id == id, cancellationToken);
    }
}
