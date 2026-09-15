using BrewInventory.Domain.Entities;
using BrewInventory.Infrastructure.Brewfather.Mappers;
using BrewInventory.Infrastructure.Brewfather.Models;
using BrewInventory.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BrewInventory.Infrastructure.Brewfather;

public class BrewfatherSyncService : IBrewfatherSyncService
{
    private readonly IBrewfatherClient _brewfatherClient;
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IRecipeRepository _recipeRepository;
    private readonly Persistence.BrewInventoryContext _dbContext;
    private readonly ILogger<BrewfatherSyncService> _logger;

    public BrewfatherSyncService(
        IBrewfatherClient brewfatherClient,
        IIngredientRepository ingredientRepository,
        IRecipeRepository recipeRepository,
        Persistence.BrewInventoryContext dbContext,
        ILogger<BrewfatherSyncService> logger)
    {
        _brewfatherClient = brewfatherClient;
        _ingredientRepository = ingredientRepository;
        _recipeRepository = recipeRepository;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SyncFermentablesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting fermentables synchronization with Brewfather");

        try
        {
            var brewfatherFermentables = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherFermentable>("inventory/fermentables", cancellationToken);

            _logger.LogInformation("Retrieved {Count} fermentables from Brewfather", brewfatherFermentables.Count);

            var existingFermentables = await _dbContext.Fermentables
                .Where(f => f.BrewfatherId != null)
                .ToDictionaryAsync(f => f.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfFermentable in brewfatherFermentables)
            {
                if (existingFermentables.TryGetValue(bfFermentable._id, out var existingFermentable))
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

            _logger.LogInformation(
                "Fermentables synchronization completed. Added: {Added}, Updated: {Updated}",
                addedCount, updatedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during fermentables synchronization");
            throw;
        }
    }

    public async Task SyncHopsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting hops synchronization with Brewfather");

        try
        {
            var brewfatherHops = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherHop>("inventory/hops", cancellationToken);

            _logger.LogInformation("Retrieved {Count} hops from Brewfather", brewfatherHops.Count);

            var existingHops = await _dbContext.Hops
                .Where(h => h.BrewfatherId != null)
                .ToDictionaryAsync(h => h.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfHop in brewfatherHops)
            {
                if (existingHops.TryGetValue(bfHop._id, out var existingHop))
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

            _logger.LogInformation(
                "Hops synchronization completed. Added: {Added}, Updated: {Updated}",
                addedCount, updatedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during hops synchronization");
            throw;
        }
    }

    public async Task SyncYeastsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting yeasts synchronization with Brewfather");

        try
        {
            var brewfatherYeasts = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherYeast>("inventory/yeasts", cancellationToken);

            _logger.LogInformation("Retrieved {Count} yeasts from Brewfather", brewfatherYeasts.Count);

            var existingYeasts = await _dbContext.Yeasts
                .Where(y => y.BrewfatherId != null)
                .ToDictionaryAsync(y => y.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfYeast in brewfatherYeasts)
            {
                if (existingYeasts.TryGetValue(bfYeast._id, out var existingYeast))
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

            _logger.LogInformation(
                "Yeasts synchronization completed. Added: {Added}, Updated: {Updated}",
                addedCount, updatedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during yeasts synchronization");
            throw;
        }
    }

    public async Task SyncMiscsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting miscs synchronization with Brewfather");

        try
        {
            var brewfatherMiscs = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherMisc>("inventory/miscs", cancellationToken);

            _logger.LogInformation("Retrieved {Count} miscs from Brewfather", brewfatherMiscs.Count);

            var existingMiscs = await _dbContext.Miscs
                .Where(m => m.BrewfatherId != null)
                .ToDictionaryAsync(m => m.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfMisc in brewfatherMiscs)
            {
                if (existingMiscs.TryGetValue(bfMisc._id, out var existingMisc))
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

            _logger.LogInformation(
                "Miscs synchronization completed. Added: {Added}, Updated: {Updated}",
                addedCount, updatedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during miscs synchronization");
            throw;
        }
    }

    public async Task SyncRecipesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting recipes synchronization with Brewfather");

        try
        {
            var brewfatherRecipes = await _brewfatherClient
                .GetAllItemsAsync<BrewfatherRecipe>("recipes", "fermentables,hops,miscs,yeasts", cancellationToken);

            _logger.LogInformation("Retrieved {Count} recipes from Brewfather", brewfatherRecipes.Count);

            var existingRecipes = await _dbContext.Recipes
                .Include(r => r.RecipeFermentables)
                .Include(r => r.RecipeHops)
                .Include(r => r.RecipeMiscs)
                .Include(r => r.RecipeYeasts)
                .Where(r => r.BrewfatherId != null)
                .ToDictionaryAsync(r => r.BrewfatherId!, cancellationToken);

            var brewfatherRecipeIds = brewfatherRecipes.Select(r => r._id).ToHashSet();

            var recipesToDelete = existingRecipes.Values
                .Where(r => !brewfatherRecipeIds.Contains(r.BrewfatherId!))
                .ToList();

            foreach (var recipeToDelete in recipesToDelete)
            {
                _dbContext.Recipes.Remove(recipeToDelete);
            }

            var fermentableLookup = await _dbContext.Fermentables
                .Where(f => f.BrewfatherId != null)
                .ToDictionaryAsync(f => f.BrewfatherId!, cancellationToken);

            var hopLookup = await _dbContext.Hops
                .Where(h => h.BrewfatherId != null)
                .ToDictionaryAsync(h => h.BrewfatherId!, cancellationToken);

            var yeastLookup = await _dbContext.Yeasts
                .Where(y => y.BrewfatherId != null)
                .ToDictionaryAsync(y => y.BrewfatherId!, cancellationToken);

            var miscLookup = await _dbContext.Miscs
                .Where(m => m.BrewfatherId != null)
                .ToDictionaryAsync(m => m.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;
            var deletedCount = recipesToDelete.Count;
            var missingIngredientsCount = 0;

            foreach (var bfRecipe in brewfatherRecipes)
            {
                missingIngredientsCount += await EnsureIngredientsExistAsync(
                    bfRecipe,
                    fermentableLookup,
                    hopLookup,
                    yeastLookup,
                    miscLookup,
                    cancellationToken);

                if (existingRecipes.TryGetValue(bfRecipe._id, out var existingRecipe))
                {
                    RecipeMapper.UpdateEntity(
                        existingRecipe,
                        bfRecipe,
                        fermentableLookup,
                        hopLookup,
                        yeastLookup,
                        miscLookup);
                    updatedCount++;
                }
                else
                {
                    var newRecipe = RecipeMapper.ToEntity(
                        bfRecipe,
                        fermentableLookup,
                        hopLookup,
                        yeastLookup,
                        miscLookup);
                    _dbContext.Recipes.Add(newRecipe);
                    addedCount++;
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Recipes synchronization completed. Added: {Added}, Updated: {Updated}, Deleted: {Deleted}, Missing Ingredients Created: {MissingIngredients}",
                addedCount, updatedCount, deletedCount, missingIngredientsCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during recipes synchronization");
            throw;
        }
    }

    private async Task<int> EnsureIngredientsExistAsync(
        BrewfatherRecipe bfRecipe,
        Dictionary<string, Fermentable> fermentableLookup,
        Dictionary<string, Hop> hopLookup,
        Dictionary<string, Yeast> yeastLookup,
        Dictionary<string, Misc> miscLookup,
        CancellationToken cancellationToken)
    {
        var createdCount = 0;

        if (bfRecipe.fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.fermentables)
            {
                if (!fermentableLookup.ContainsKey(bfFermentable._id))
                {
                    var newFermentable = FermentableMapper.ToEntityFromRecipe(bfFermentable);
                    newFermentable.Amount = 0;
                    _dbContext.Fermentables.Add(newFermentable);
                    fermentableLookup[bfFermentable._id] = newFermentable;
                    createdCount++;
                    _logger.LogInformation("Created missing fermentable: {Name} with BrewfatherId: {Id}", newFermentable.Name, newFermentable.BrewfatherId);
                }
            }
        }

        if (bfRecipe.hops != null)
        {
            foreach (var bfHop in bfRecipe.hops)
            {
                if (!hopLookup.ContainsKey(bfHop._id))
                {
                    var newHop = HopMapper.ToEntityFromRecipe(bfHop);
                    newHop.Amount = 0;
                    _dbContext.Hops.Add(newHop);
                    hopLookup[bfHop._id] = newHop;
                    createdCount++;
                    _logger.LogInformation("Created missing hop: {Name} with BrewfatherId: {Id}", newHop.Name, newHop.BrewfatherId);
                }
            }
        }

        if (bfRecipe.yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.yeasts)
            {
                if (!yeastLookup.ContainsKey(bfYeast._id))
                {
                    var newYeast = YeastMapper.ToEntityFromRecipe(bfYeast);
                    newYeast.Amount = 0;
                    _dbContext.Yeasts.Add(newYeast);
                    yeastLookup[bfYeast._id] = newYeast;
                    createdCount++;
                    _logger.LogInformation("Created missing yeast: {Name} with BrewfatherId: {Id}", newYeast.Name, newYeast.BrewfatherId);
                }
            }
        }

        if (bfRecipe.miscs != null)
        {
            foreach (var bfMisc in bfRecipe.miscs)
            {
                if (!miscLookup.ContainsKey(bfMisc._id))
                {
                    var newMisc = MiscMapper.ToEntityFromRecipe(bfMisc);
                    newMisc.Amount = 0;
                    _dbContext.Miscs.Add(newMisc);
                    miscLookup[bfMisc._id] = newMisc;
                    createdCount++;
                    _logger.LogInformation("Created missing misc: {Name} with BrewfatherId: {Id}", newMisc.Name, newMisc.BrewfatherId);
                }
            }
        }

        if (createdCount > 0)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return createdCount;
    }

    public async Task<Recipe> PushRecipeToBrewfatherAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting push of recipe {RecipeId} to Brewfather", recipeId);

        var recipe = await _recipeRepository.GetByIdAsync(recipeId, cancellationToken);

        if (recipe is null)
        {
            throw new KeyNotFoundException($"Recipe {recipeId} does not exist.");
        }

        if (!string.IsNullOrWhiteSpace(recipe.BrewfatherId))
        {
            throw new InvalidOperationException($"Recipe {recipeId} has already been created in Brewfather.");
        }

        var missingIngredients = RecipeMapper.GetIngredientsWithoutBrewfatherId(recipe);
        if (missingIngredients.Count > 0)
        {
            var details = string.Join(", ", missingIngredients);
            throw new InvalidOperationException(
                $"Recipe contains ingredients that are not yet known to Brewfather: {details}. " +
                "Sync these ingredients to Brewfather first.");
        }

        var brewfatherRequest = RecipeMapper.ToBrewfatherRequest(recipe);

        var brewfatherId = await _brewfatherClient.CreateRecipeAsync(brewfatherRequest, cancellationToken);

        recipe.BrewfatherId = brewfatherId;
        await _recipeRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Recipe {RecipeId} pushed to Brewfather with id {BrewfatherId}",
            recipeId, brewfatherId);

        return recipe;
    }
}
