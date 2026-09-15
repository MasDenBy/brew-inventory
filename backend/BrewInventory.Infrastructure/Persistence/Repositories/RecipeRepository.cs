using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.Infrastructure.Persistence.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly BrewInventoryContext _context;

    public RecipeRepository(BrewInventoryContext context)
    {
        _context = context;
    }

    public async Task<List<Recipe>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Recipes.ToListAsync(cancellationToken);
    }

    public async Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.RecipeFermentables)
                .ThenInclude(rf => rf.Fermentable)
            .Include(r => r.RecipeHops)
                .ThenInclude(rh => rh.Hop)
            .Include(r => r.RecipeYeasts)
                .ThenInclude(ry => ry.Yeast)
            .Include(r => r.RecipeMiscs)
                .ThenInclude(rm => rm.Misc)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<List<Recipe>> GetByIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.RecipeFermentables)
                .ThenInclude(rf => rf.Fermentable)
            .Include(r => r.RecipeHops)
                .ThenInclude(rh => rh.Hop)
            .Include(r => r.RecipeYeasts)
                .ThenInclude(ry => ry.Yeast)
            .Include(r => r.RecipeMiscs)
                .ThenInclude(rm => rm.Misc)
            .Where(r => ids.Contains(r.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Recipe> AddAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync(cancellationToken);
        return recipe;
    }

    public async Task DeleteAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<int>> GetExistingFermentableIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Fermentables
            .Where(f => ids.Contains(f.Id))
            .Select(f => f.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<int>> GetExistingHopIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Hops
            .Where(h => ids.Contains(h.Id))
            .Select(h => h.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<int>> GetExistingYeastIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Yeasts
            .Where(y => ids.Contains(y.Id))
            .Select(y => y.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<int>> GetExistingMiscIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Miscs
            .Where(m => ids.Contains(m.Id))
            .Select(m => m.Id)
            .ToListAsync(cancellationToken);
    }
}
