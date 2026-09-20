using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;
using BrewInventory.Infrastructure.Brewfather.Mappers;
using BrewInventory.Infrastructure.Brewfather.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.Infrastructure.Brewfather;

public class BrewfatherSyncService : IBrewfatherSyncService
{
    private readonly IBrewfatherClient _brewfatherClient;
    private readonly IRecipeRepository _recipeRepository;
    private readonly Persistence.BrewInventoryContext _dbContext;

    public BrewfatherSyncService(
        IBrewfatherClient brewfatherClient,
        IRecipeRepository recipeRepository,
        Persistence.BrewInventoryContext dbContext)
    {
        _brewfatherClient = brewfatherClient;
        _recipeRepository = recipeRepository;
        _dbContext = dbContext;
    }

    public async Task SyncFermentablesAsync(CancellationToken cancellationToken = default)
    {
        var brewfatherFermentables = await _brewfatherClient
            .GetAllItemsAsync<BrewfatherFermentable>("inventory/fermentables", "grainCategory,percentage,color,lovibond,origin", cancellationToken);

        var existingFermentables = await _dbContext.Fermentables
            .Where(f => f.BrewfatherId != null)
            .ToDictionaryAsync(f => f.BrewfatherId!, cancellationToken);

        var addedCount = 0;
        var updatedCount = 0;

        foreach (var bfFermentable in brewfatherFermentables)
        {
            if (existingFermentables.TryGetValue(bfFermentable.Id, out var existingFermentable))
            {
                FermentableMapper.UpdateEntity(existingFermentable, bfFermentable);
                updatedCount++;
            }
            else
            {
                var newFermentable = FermentableMapper.ToEntity(bfFermentable);
                _dbContext.Fermentables.Add(newFermentable);
                addedCount++;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncHopsAsync(CancellationToken cancellationToken = default)
    {
        var brewfatherHops = await _brewfatherClient
            .GetAllItemsAsync<BrewfatherHop>("inventory/hops", "origin,usage,year", cancellationToken);

        var existingHops = await _dbContext.Hops
            .Where(h => h.BrewfatherId != null)
            .ToDictionaryAsync(h => h.BrewfatherId!, cancellationToken);

        var addedCount = 0;
        var updatedCount = 0;

        foreach (var bfHop in brewfatherHops)
        {
            if (existingHops.TryGetValue(bfHop.Id, out var existingHop))
            {
                HopMapper.UpdateEntity(existingHop, bfHop);
                updatedCount++;
            }
            else
            {
                var newHop = HopMapper.ToEntity(bfHop);
                _dbContext.Hops.Add(newHop);
                addedCount++;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncYeastsAsync(CancellationToken cancellationToken = default)
    {
        var brewfatherYeasts = await _brewfatherClient
            .GetAllItemsAsync<BrewfatherYeast>("inventory/yeasts", "laboratory,form,unit,productId", cancellationToken);

        var existingYeasts = await _dbContext.Yeasts
            .Where(y => y.BrewfatherId != null)
            .ToDictionaryAsync(y => y.BrewfatherId!, cancellationToken);

        var addedCount = 0;
        var updatedCount = 0;

        foreach (var bfYeast in brewfatherYeasts)
        {
            if (existingYeasts.TryGetValue(bfYeast.Id, out var existingYeast))
            {
                YeastMapper.UpdateEntity(existingYeast, bfYeast);
                updatedCount++;
            }
            else
            {
                var newYeast = YeastMapper.ToEntity(bfYeast);
                _dbContext.Yeasts.Add(newYeast);
                addedCount++;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncMiscsAsync(CancellationToken cancellationToken = default)
    {
        var brewfatherMiscs = await _brewfatherClient
            .GetAllItemsAsync<BrewfatherMisc>("inventory/miscs", "unit", cancellationToken);

        var existingMiscs = await _dbContext.Miscs
            .Where(m => m.BrewfatherId != null)
            .ToDictionaryAsync(m => m.BrewfatherId!, cancellationToken);

        var addedCount = 0;
        var updatedCount = 0;

        foreach (var bfMisc in brewfatherMiscs)
        {
            if (existingMiscs.TryGetValue(bfMisc.Id, out var existingMisc))
            {
                MiscMapper.UpdateEntity(existingMisc, bfMisc);
                updatedCount++;
            }
            else
            {
                var newMisc = MiscMapper.ToEntity(bfMisc);
                _dbContext.Miscs.Add(newMisc);
                addedCount++;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SyncRecipesAsync(CancellationToken cancellationToken = default)
    {
        var brewfatherRecipes = await _brewfatherClient
            .GetAllItemsAsync<BrewfatherRecipe>("recipes", "fermentables,hops,miscs,yeasts", cancellationToken);

        var existingRecipes = await _dbContext.Recipes
            .Include(r => r.RecipeFermentables)
            .Include(r => r.RecipeHops)
            .Include(r => r.RecipeMiscs)
            .Include(r => r.RecipeYeasts)
            .Where(r => r.BrewfatherId != null)
            .ToDictionaryAsync(r => r.BrewfatherId!, cancellationToken);

        var brewfatherRecipeIds = brewfatherRecipes.Select(r => r.Id)
            .ToHashSet(StringComparer.InvariantCulture);

        var recipesToDelete = existingRecipes.Values
            .Where(r => !brewfatherRecipeIds.Contains(r.BrewfatherId!))
            .ToList();

        foreach (var recipeToDelete in recipesToDelete)
        {
            _dbContext.Recipes.Remove(recipeToDelete);
        }

        foreach (var bfRecipe in brewfatherRecipes)
        {
            if (existingRecipes.TryGetValue(bfRecipe.Id, out var existingRecipe))
            {
                RecipeMapper.UpdateEntity(existingRecipe, bfRecipe);
            }
            else
            {
                var newRecipe = RecipeMapper.ToEntity(bfRecipe);
                _dbContext.Recipes.Add(newRecipe);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Recipe> PushRecipeToBrewfatherAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        var recipe = await _recipeRepository.GetByIdAsync(recipeId, cancellationToken);

        if (recipe is null)
        {
            throw new KeyNotFoundException($"Recipe {recipeId} does not exist.");
        }

        if (!string.IsNullOrWhiteSpace(recipe.BrewfatherId))
        {
            throw new InvalidOperationException($"Recipe {recipeId} has already been created in Brewfather.");
        }

        var brewfatherRequest = RecipeMapper.ToBrewfatherRequest(recipe);

        var brewfatherId = await _brewfatherClient.CreateRecipeAsync(brewfatherRequest, cancellationToken);

        recipe.BrewfatherId = brewfatherId;
        await _recipeRepository.SaveChangesAsync(cancellationToken);

        return recipe;
    }
}
