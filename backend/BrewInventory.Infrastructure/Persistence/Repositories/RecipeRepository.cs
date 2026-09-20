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

    public async Task<ICollection<Recipe>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Recipes.ToListAsync(cancellationToken);
    }

    public async Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.RecipeFermentables)
            .Include(r => r.RecipeHops)
            .Include(r => r.RecipeYeasts)
            .Include(r => r.RecipeMiscs)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<ICollection<Recipe>> GetByIdsAsync(ICollection<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Recipes
            .Include(r => r.RecipeFermentables)
            .Include(r => r.RecipeHops)
            .Include(r => r.RecipeYeasts)
            .Include(r => r.RecipeMiscs)
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
}
