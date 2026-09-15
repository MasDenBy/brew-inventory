using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.Infrastructure.Persistence.Repositories;

public class IngredientRepository : IIngredientRepository
{
    private readonly BrewInventoryContext _context;

    public IngredientRepository(BrewInventoryContext context)
    {
        _context = context;
    }

    public async Task<List<Fermentable>> GetAllFermentablesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Fermentables.ToListAsync(cancellationToken);
    }

    public async Task<Fermentable?> GetFermentableByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Fermentables.FindAsync([id], cancellationToken);
    }

    public async Task<Fermentable> AddFermentableAsync(Fermentable fermentable, CancellationToken cancellationToken = default)
    {
        _context.Fermentables.Add(fermentable);
        await _context.SaveChangesAsync(cancellationToken);
        return fermentable;
    }

    public async Task UpdateFermentableAsync(Fermentable fermentable, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFermentableAsync(Fermentable fermentable, CancellationToken cancellationToken = default)
    {
        _context.Fermentables.Remove(fermentable);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Hop>> GetAllHopsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Hops.ToListAsync(cancellationToken);
    }

    public async Task<Hop?> GetHopByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Hops.FindAsync([id], cancellationToken);
    }

    public async Task<Hop> AddHopAsync(Hop hop, CancellationToken cancellationToken = default)
    {
        _context.Hops.Add(hop);
        await _context.SaveChangesAsync(cancellationToken);
        return hop;
    }

    public async Task UpdateHopAsync(Hop hop, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteHopAsync(Hop hop, CancellationToken cancellationToken = default)
    {
        _context.Hops.Remove(hop);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Yeast>> GetAllYeastsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Yeasts.ToListAsync(cancellationToken);
    }

    public async Task<Yeast?> GetYeastByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Yeasts.FindAsync([id], cancellationToken);
    }

    public async Task<Yeast> AddYeastAsync(Yeast yeast, CancellationToken cancellationToken = default)
    {
        _context.Yeasts.Add(yeast);
        await _context.SaveChangesAsync(cancellationToken);
        return yeast;
    }

    public async Task UpdateYeastAsync(Yeast yeast, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteYeastAsync(Yeast yeast, CancellationToken cancellationToken = default)
    {
        _context.Yeasts.Remove(yeast);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Misc>> GetAllMiscsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Miscs.ToListAsync(cancellationToken);
    }

    public async Task<Misc?> GetMiscByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Miscs.FindAsync([id], cancellationToken);
    }

    public async Task<Misc> AddMiscAsync(Misc misc, CancellationToken cancellationToken = default)
    {
        _context.Miscs.Add(misc);
        await _context.SaveChangesAsync(cancellationToken);
        return misc;
    }

    public async Task UpdateMiscAsync(Misc misc, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteMiscAsync(Misc misc, CancellationToken cancellationToken = default)
    {
        _context.Miscs.Remove(misc);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsFermentableAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Fermentables.AnyAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsHopAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Hops.AnyAsync(h => h.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsYeastAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Yeasts.AnyAsync(y => y.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsMiscAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Miscs.AnyAsync(m => m.Id == id, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
