using BrewInventory.App.Data;
using BrewInventory.App.Data.Entities;
using BrewInventory.App.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewInventory.App.Services;

public class BrewfatherSyncService : IBrewfatherSyncService
{
    private readonly IBrewfatherClient _brewfatherClient;
    private readonly BrewInventoryContext _dbContext;
    private readonly ILogger<BrewfatherSyncService> _logger;

    public BrewfatherSyncService(
        IBrewfatherClient brewfatherClient,
        BrewInventoryContext dbContext,
        ILogger<BrewfatherSyncService> logger)
    {
        _brewfatherClient = brewfatherClient;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task SyncFermentablesAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting fermentables synchronization with Brewfather");

        try
        {
            // Fetch all fermentables from Brewfather
            var brewfatherFermentables = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherFermentable>("inventory/fermentables", cancellationToken);

            _logger.LogInformation("Retrieved {Count} fermentables from Brewfather", brewfatherFermentables.Count);

            // Get all existing fermentables from database
            var existingFermentables = await _dbContext.Fermentables
                .Where(f => f.BrewfatherId != null)
                .ToDictionaryAsync(f => f.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfFermentable in brewfatherFermentables)
            {
                if (existingFermentables.TryGetValue(bfFermentable._id, out var existingFermentable))
                {
                    // Update existing fermentable
                    UpdateFermentableFromBrewfather(existingFermentable, bfFermentable);
                    updatedCount++;
                }
                else
                {
                    // Add new fermentable
                    var newFermentable = CreateFermentableFromBrewfather(bfFermentable);
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
            // Fetch all hops from Brewfather
            var brewfatherHops = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherHop>("inventory/hops", cancellationToken);

            _logger.LogInformation("Retrieved {Count} hops from Brewfather", brewfatherHops.Count);

            // Get all existing hops from database
            var existingHops = await _dbContext.Hops
                .Where(h => h.BrewfatherId != null)
                .ToDictionaryAsync(h => h.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfHop in brewfatherHops)
            {
                if (existingHops.TryGetValue(bfHop._id, out var existingHop))
                {
                    // Update existing hop
                    UpdateHopFromBrewfather(existingHop, bfHop);
                    updatedCount++;
                }
                else
                {
                    // Add new hop
                    var newHop = CreateHopFromBrewfather(bfHop);
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
            // Fetch all yeasts from Brewfather
            var brewfatherYeasts = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherYeast>("inventory/yeasts", cancellationToken);

            _logger.LogInformation("Retrieved {Count} yeasts from Brewfather", brewfatherYeasts.Count);

            // Get all existing yeasts from database
            var existingYeasts = await _dbContext.Yeasts
                .Where(y => y.BrewfatherId != null)
                .ToDictionaryAsync(y => y.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfYeast in brewfatherYeasts)
            {
                if (existingYeasts.TryGetValue(bfYeast._id, out var existingYeast))
                {
                    // Update existing yeast
                    UpdateYeastFromBrewfather(existingYeast, bfYeast);
                    updatedCount++;
                }
                else
                {
                    // Add new yeast
                    var newYeast = CreateYeastFromBrewfather(bfYeast);
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
            // Fetch all miscs from Brewfather
            var brewfatherMiscs = await _brewfatherClient
                .GetAllInventoryItemsAsync<BrewfatherMisc>("inventory/miscs", cancellationToken);

            _logger.LogInformation("Retrieved {Count} miscs from Brewfather", brewfatherMiscs.Count);

            // Get all existing miscs from database
            var existingMiscs = await _dbContext.Miscs
                .Where(m => m.BrewfatherId != null)
                .ToDictionaryAsync(m => m.BrewfatherId!, cancellationToken);

            var addedCount = 0;
            var updatedCount = 0;

            foreach (var bfMisc in brewfatherMiscs)
            {
                if (existingMiscs.TryGetValue(bfMisc._id, out var existingMisc))
                {
                    // Update existing misc
                    UpdateMiscFromBrewfather(existingMisc, bfMisc);
                    updatedCount++;
                }
                else
                {
                    // Add new misc
                    var newMisc = CreateMiscFromBrewfather(bfMisc);
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
            // Fetch all recipes from Brewfather with ingredients
            var brewfatherRecipes = await _brewfatherClient
                .GetAllItemsAsync<BrewfatherRecipe>("recipes", "fermentables,hops,miscs,yeasts", cancellationToken);

            _logger.LogInformation("Retrieved {Count} recipes from Brewfather", brewfatherRecipes.Count);

            // Get all existing recipes from database
            var existingRecipes = await _dbContext.Recipes
                .Include(r => r.RecipeFermentables)
                .Include(r => r.RecipeHops)
                .Include(r => r.RecipeMiscs)
                .Include(r => r.RecipeYeasts)
                .Where(r => r.BrewfatherId != null)
                .ToDictionaryAsync(r => r.BrewfatherId!, cancellationToken);

            // Get brewfather IDs that still exist in Brewfather
            var brewfatherRecipeIds = brewfatherRecipes.Select(r => r._id).ToHashSet();

            // Remove recipes that no longer exist in Brewfather
            var recipesToDelete = existingRecipes.Values
                .Where(r => !brewfatherRecipeIds.Contains(r.BrewfatherId!))
                .ToList();

            foreach (var recipeToDelete in recipesToDelete)
            {
                _dbContext.Recipes.Remove(recipeToDelete);
            }

            // Get inventory lookups for matching ingredients
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
                // Create missing ingredients before processing recipe
                missingIngredientsCount += await EnsureIngredientsExistAsync(
                    bfRecipe,
                    fermentableLookup,
                    hopLookup,
                    yeastLookup,
                    miscLookup,
                    cancellationToken);

                if (existingRecipes.TryGetValue(bfRecipe._id, out var existingRecipe))
                {
                    // Update existing recipe
                    UpdateRecipeFromBrewfather(
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
                    // Add new recipe
                    var newRecipe = CreateRecipeFromBrewfather(
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

        // Check and create missing fermentables
        if (bfRecipe.fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.fermentables)
            {
                if (!fermentableLookup.ContainsKey(bfFermentable._id))
                {
                    var newFermentable = new Fermentable
                    {
                        Name = bfFermentable.name,
                        Amount = 0, // Set amount to 0 as ingredient is not in inventory
                        BrewfatherId = bfFermentable._id,
                        Supplier = string.IsNullOrWhiteSpace(bfFermentable.supplier) ? null : bfFermentable.supplier,
                        Origin = string.IsNullOrWhiteSpace(bfFermentable.origin) ? null : bfFermentable.origin,
                        Type = MapBrewfatherType(bfFermentable.type),
                        Color = bfFermentable.color ?? 0,
                        BestBefore = null
                    };
                    _dbContext.Fermentables.Add(newFermentable);
                    fermentableLookup[bfFermentable._id] = newFermentable;
                    createdCount++;
                    _logger.LogInformation("Created missing fermentable: {Name} with BrewfatherId: {Id}", newFermentable.Name, newFermentable.BrewfatherId);
                }
            }
        }

        // Check and create missing hops
        if (bfRecipe.hops != null)
        {
            foreach (var bfHop in bfRecipe.hops)
            {
                if (!hopLookup.ContainsKey(bfHop._id))
                {
                    var newHop = new Hop
                    {
                        Name = bfHop.name,
                        Amount = 0, // Set amount to 0 as ingredient is not in inventory
                        BrewfatherId = bfHop._id,
                        AlphaAcid = bfHop.alpha ?? 0,
                        Type = MapBrewfatherHopType(bfHop.type),
                        Origin = string.IsNullOrWhiteSpace(bfHop.origin) ? null : bfHop.origin,
                        HarvestYear = null,
                        BestBefore = null
                    };
                    _dbContext.Hops.Add(newHop);
                    hopLookup[bfHop._id] = newHop;
                    createdCount++;
                    _logger.LogInformation("Created missing hop: {Name} with BrewfatherId: {Id}", newHop.Name, newHop.BrewfatherId);
                }
            }
        }

        // Check and create missing yeasts
        if (bfRecipe.yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.yeasts)
            {
                if (!yeastLookup.ContainsKey(bfYeast._id))
                {
                    var newYeast = new Yeast
                    {
                        Name = bfYeast.name,
                        Amount = 0, // Set amount to 0 as ingredient is not in inventory
                        BrewfatherId = bfYeast._id,
                        Type = MapBrewfatherYeastType(bfYeast.type),
                        Labaratory = string.IsNullOrWhiteSpace(bfYeast.laboratory) ? "Unknown" : bfYeast.laboratory,
                        Form = MapBrewfatherYeastForm(bfYeast.form),
                        BestBefore = null
                    };
                    _dbContext.Yeasts.Add(newYeast);
                    yeastLookup[bfYeast._id] = newYeast;
                    createdCount++;
                    _logger.LogInformation("Created missing yeast: {Name} with BrewfatherId: {Id}", newYeast.Name, newYeast.BrewfatherId);
                }
            }
        }

        // Check and create missing miscs
        if (bfRecipe.miscs != null)
        {
            foreach (var bfMisc in bfRecipe.miscs)
            {
                if (!miscLookup.ContainsKey(bfMisc._id))
                {
                    var newMisc = new Misc
                    {
                        Name = bfMisc.name,
                        Amount = 0, // Set amount to 0 as ingredient is not in inventory
                        BrewfatherId = bfMisc._id,
                        Type = MapBrewfatherMiscType(bfMisc.type),
                        Unit = MapBrewfatherMiscUnit(bfMisc.unit),
                        BestBefore = null
                    };
                    _dbContext.Miscs.Add(newMisc);
                    miscLookup[bfMisc._id] = newMisc;
                    createdCount++;
                    _logger.LogInformation("Created missing misc: {Name} with BrewfatherId: {Id}", newMisc.Name, newMisc.BrewfatherId);
                }
            }
        }

        // Save all new ingredients immediately so they have IDs for the recipe
        if (createdCount > 0)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return createdCount;
    }

    private Recipe CreateRecipeFromBrewfather(
        BrewfatherRecipe bfRecipe,
        Dictionary<string, Fermentable> fermentableLookup,
        Dictionary<string, Hop> hopLookup,
        Dictionary<string, Yeast> yeastLookup,
        Dictionary<string, Misc> miscLookup)
    {
        var recipe = new Recipe
        {
            Name = bfRecipe.name,
            BrewfatherId = bfRecipe._id
        };

        // Add fermentables
        if (bfRecipe.fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.fermentables)
            {
                if (fermentableLookup.TryGetValue(bfFermentable._id, out var fermentable))
                {
                    recipe.RecipeFermentables.Add(new RecipeFermentable
                    {
                        FermentableId = fermentable.Id,
                        Amount = (decimal)bfFermentable.amount
                    });
                }
            }
        }

        // Add hops
        if (bfRecipe.hops != null)
        {
            foreach (var bfHop in bfRecipe.hops)
            {
                if (hopLookup.TryGetValue(bfHop._id, out var hop))
                {
                    recipe.RecipeHops.Add(new RecipeHop
                    {
                        HopId = hop.Id,
                        Amount = (decimal)bfHop.amount
                    });
                }
            }
        }

        // Add yeasts
        if (bfRecipe.yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.yeasts)
            {
                if (yeastLookup.TryGetValue(bfYeast._id, out var yeast))
                {
                    recipe.RecipeYeasts.Add(new RecipeYeast
                    {
                        YeastId = yeast.Id,
                        Amount = (decimal)bfYeast.amount
                    });
                }
            }
        }

        // Add miscs
        if (bfRecipe.miscs != null)
        {
            foreach (var bfMisc in bfRecipe.miscs)
            {
                if (miscLookup.TryGetValue(bfMisc._id, out var misc))
                {
                    recipe.RecipeMiscs.Add(new RecipeMisc
                    {
                        MiscId = misc.Id,
                        Amount = (decimal)bfMisc.amount
                    });
                }
            }
        }

        return recipe;
    }

    private void UpdateRecipeFromBrewfather(
        Recipe existingRecipe,
        BrewfatherRecipe bfRecipe,
        Dictionary<string, Fermentable> fermentableLookup,
        Dictionary<string, Hop> hopLookup,
        Dictionary<string, Yeast> yeastLookup,
        Dictionary<string, Misc> miscLookup)
    {
        existingRecipe.Name = bfRecipe.name;

        // Clear existing ingredients
        existingRecipe.RecipeFermentables.Clear();
        existingRecipe.RecipeHops.Clear();
        existingRecipe.RecipeYeasts.Clear();
        existingRecipe.RecipeMiscs.Clear();

        // Add fermentables
        if (bfRecipe.fermentables != null)
        {
            foreach (var bfFermentable in bfRecipe.fermentables)
            {
                if (fermentableLookup.TryGetValue(bfFermentable._id, out var fermentable))
                {
                    existingRecipe.RecipeFermentables.Add(new RecipeFermentable
                    {
                        FermentableId = fermentable.Id,
                        Amount = (decimal)bfFermentable.amount
                    });
                }
            }
        }

        // Add hops
        if (bfRecipe.hops != null)
        {
            foreach (var bfHop in bfRecipe.hops)
            {
                if (hopLookup.TryGetValue(bfHop._id, out var hop))
                {
                    existingRecipe.RecipeHops.Add(new RecipeHop
                    {
                        HopId = hop.Id,
                        Amount = (decimal)bfHop.amount
                    });
                }
            }
        }

        // Add yeasts
        if (bfRecipe.yeasts != null)
        {
            foreach (var bfYeast in bfRecipe.yeasts)
            {
                if (yeastLookup.TryGetValue(bfYeast._id, out var yeast))
                {
                    existingRecipe.RecipeYeasts.Add(new RecipeYeast
                    {
                        YeastId = yeast.Id,
                        Amount = (decimal)bfYeast.amount
                    });
                }
            }
        }

        // Add miscs
        if (bfRecipe.miscs != null)
        {
            foreach (var bfMisc in bfRecipe.miscs)
            {
                if (miscLookup.TryGetValue(bfMisc._id, out var misc))
                {
                    existingRecipe.RecipeMiscs.Add(new RecipeMisc
                    {
                        MiscId = misc.Id,
                        Amount = (decimal)bfMisc.amount
                    });
                }
            }
        }
    }

    private Fermentable CreateFermentableFromBrewfather(BrewfatherFermentable bfFermentable)
    {
        return new Fermentable
        {
            Name = bfFermentable.name,
            Amount = (decimal)bfFermentable.inventory,
            BrewfatherId = bfFermentable._id,
            Supplier = string.IsNullOrWhiteSpace(bfFermentable.supplier) ? null : bfFermentable.supplier,
            Type = MapBrewfatherType(bfFermentable.type),
            Color = 0, // Brewfather doesn't provide color in inventory endpoint
            Origin = null,
            BestBefore = null
        };
    }

    private void UpdateFermentableFromBrewfather(Fermentable existingFermentable, BrewfatherFermentable bfFermentable)
    {
        existingFermentable.Name = bfFermentable.name;
        existingFermentable.Amount = (decimal)bfFermentable.inventory;
        existingFermentable.Supplier = string.IsNullOrWhiteSpace(bfFermentable.supplier) ? null : bfFermentable.supplier;
        existingFermentable.Type = MapBrewfatherType(bfFermentable.type);
        // Keep existing BestBefore, Color, and Origin values
    }

    private Hop CreateHopFromBrewfather(BrewfatherHop bfHop)
    {
        return new Hop
        {
            Name = bfHop.name,
            Amount = (decimal)bfHop.inventory,
            BrewfatherId = bfHop._id,
            AlphaAcid = bfHop.alpha,
            Type = MapBrewfatherHopType(bfHop.type),
            Origin = null, // Not provided by Brewfather inventory endpoint
            HarvestYear = null, // Not provided by Brewfather inventory endpoint
            BestBefore = null
        };
    }

    private void UpdateHopFromBrewfather(Hop existingHop, BrewfatherHop bfHop)
    {
        existingHop.Name = bfHop.name;
        existingHop.Amount = (decimal)bfHop.inventory;
        existingHop.AlphaAcid = bfHop.alpha;
        existingHop.Type = MapBrewfatherHopType(bfHop.type);
        // Keep existing Origin, HarvestYear, and BestBefore values
    }

    private Yeast CreateYeastFromBrewfather(BrewfatherYeast bfYeast)
    {
        return new Yeast
        {
            Name = bfYeast.name,
            Amount = (decimal)bfYeast.inventory,
            BrewfatherId = bfYeast._id,
            Type = MapBrewfatherYeastType(bfYeast.type),
            Labaratory = string.Empty, // Not provided by Brewfather inventory endpoint
            Form = YeastForm.Liquid, // Default to Liquid as it's not provided
            BestBefore = null
        };
    }

    private void UpdateYeastFromBrewfather(Yeast existingYeast, BrewfatherYeast bfYeast)
    {
        existingYeast.Name = bfYeast.name;
        existingYeast.Amount = (decimal)bfYeast.inventory;
        existingYeast.Type = MapBrewfatherYeastType(bfYeast.type);
        // Keep existing Labaratory, Form, and BestBefore values
    }

    private Misc CreateMiscFromBrewfather(BrewfatherMisc bfMisc)
    {
        return new Misc
        {
            Name = bfMisc.name,
            Amount = (decimal)bfMisc.inventory,
            BrewfatherId = bfMisc._id,
            Type = MapBrewfatherMiscType(bfMisc.type),
            Unit = InventoryUnit.Grams, // Default to Grams as it's not provided
            BestBefore = null
        };
    }

    private void UpdateMiscFromBrewfather(Misc existingMisc, BrewfatherMisc bfMisc)
    {
        existingMisc.Name = bfMisc.name;
        existingMisc.Amount = (decimal)bfMisc.inventory;
        existingMisc.Type = MapBrewfatherMiscType(bfMisc.type);
        // Keep existing Unit and BestBefore values
    }

    public async Task<Recipe> PushRecipeToBrewfatherAsync(int recipeId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting push of recipe {RecipeId} to Brewfather", recipeId);

        var recipe = await _dbContext.Recipes
            .Include(r => r.RecipeFermentables)
                .ThenInclude(rf => rf.Fermentable)
            .Include(r => r.RecipeHops)
                .ThenInclude(rh => rh.Hop)
            .Include(r => r.RecipeYeasts)
                .ThenInclude(ry => ry.Yeast)
            .Include(r => r.RecipeMiscs)
                .ThenInclude(rm => rm.Misc)
            .FirstOrDefaultAsync(r => r.Id == recipeId, cancellationToken);

        if (recipe is null)
        {
            throw new KeyNotFoundException($"Recipe {recipeId} does not exist.");
        }

        if (!string.IsNullOrWhiteSpace(recipe.BrewfatherId))
        {
            throw new InvalidOperationException($"Recipe {recipeId} has already been created in Brewfather.");
        }

        var missingIngredients = GetIngredientsWithoutBrewfatherId(recipe);
        if (missingIngredients.Count > 0)
        {
            var details = string.Join(", ", missingIngredients);
            throw new InvalidOperationException(
                $"Recipe contains ingredients that are not yet known to Brewfather: {details}. " +
                "Sync these ingredients to Brewfather first.");
        }

        var brewfatherRequest = CreateBrewfatherRecipeRequest(recipe);

        var brewfatherId = await _brewfatherClient.CreateRecipeAsync(brewfatherRequest, cancellationToken);

        recipe.BrewfatherId = brewfatherId;
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Recipe {RecipeId} pushed to Brewfather with id {BrewfatherId}",
            recipeId, brewfatherId);

        return recipe;
    }

    private static List<string> GetIngredientsWithoutBrewfatherId(Recipe recipe)
    {
        var missing = new List<string>();

        foreach (var rf in recipe.RecipeFermentables)
        {
            if (string.IsNullOrWhiteSpace(rf.Fermentable.BrewfatherId))
            {
                missing.Add($"fermentable '{rf.Fermentable.Name}'");
            }
        }

        foreach (var rh in recipe.RecipeHops)
        {
            if (string.IsNullOrWhiteSpace(rh.Hop.BrewfatherId))
            {
                missing.Add($"hop '{rh.Hop.Name}'");
            }
        }

        foreach (var ry in recipe.RecipeYeasts)
        {
            if (string.IsNullOrWhiteSpace(ry.Yeast.BrewfatherId))
            {
                missing.Add($"yeast '{ry.Yeast.Name}'");
            }
        }

        foreach (var rm in recipe.RecipeMiscs)
        {
            if (string.IsNullOrWhiteSpace(rm.Misc.BrewfatherId))
            {
                missing.Add($"misc '{rm.Misc.Name}'");
            }
        }

        return missing;
    }

    private static BrewfatherCreateRecipeRequest CreateBrewfatherRecipeRequest(Recipe recipe)
    {
        return new BrewfatherCreateRecipeRequest(
            recipe.Name,
            "All Grain",
            recipe.RecipeFermentables.Select(rf => new BrewfatherRecipeFermentable(
                rf.Fermentable.BrewfatherId!, // validated earlier
                (double)rf.Amount,
                rf.Fermentable.Name,
                MapToBrewfatherFermentableType(rf.Fermentable.Type),
                rf.Fermentable.Supplier,
                rf.Fermentable.Origin,
                rf.Fermentable.Color,
                null
            )).ToList(),
            recipe.RecipeHops.Select(rh => new BrewfatherRecipeHop(
                rh.Hop.BrewfatherId!, // validated earlier
                (double)rh.Amount,
                rh.Hop.Name,
                rh.Hop.AlphaAcid,
                MapToBrewfatherHopType(rh.Hop.Type),
                rh.Hop.Origin,
                "Boil",
                60
            )).ToList(),
            recipe.RecipeMiscs.Select(rm => new BrewfatherRecipeMisc(
                rm.Misc.BrewfatherId!, // validated earlier
                (double)rm.Amount,
                rm.Misc.Name,
                MapToBrewfatherMiscType(rm.Misc.Type),
                MapToBrewfatherMiscUnit(rm.Misc.Unit),
                "Boil",
                0
            )).ToList(),
            recipe.RecipeYeasts.Select(ry => new BrewfatherRecipeYeast(
                ry.Yeast.BrewfatherId!, // validated earlier
                (double)ry.Amount,
                ry.Yeast.Name,
                string.IsNullOrWhiteSpace(ry.Yeast.Labaratory) ? null : ry.Yeast.Labaratory,
                MapToBrewfatherYeastType(ry.Yeast.Type),
                MapToBrewfatherYeastForm(ry.Yeast.Form),
                null,
                "pkg"
            )).ToList()
        );
    }

    private static string MapToBrewfatherFermentableType(FermentableType type)
    {
        return type switch
        {
            FermentableType.Grain => "Grain",
            FermentableType.Sugar => "Sugar",
            FermentableType.LiquidExtract => "Extract",
            FermentableType.DryExtract => "Dry Extract",
            FermentableType.Adjunct => "Adjunct",
            _ => "Other"
        };
    }

    private static string MapToBrewfatherHopType(HopType type)
    {
        return type switch
        {
            HopType.Pellet => "Pellet",
            HopType.Whole => "Whole",
            HopType.Cryo => "Cryo",
            HopType.CO2Extract => "Co2 Extract",
            _ => "Pellet"
        };
    }

    private static string MapToBrewfatherYeastType(YeastType type)
    {
        return type switch
        {
            YeastType.Ale => "Ale",
            YeastType.Lager => "Lager",
            YeastType.Hybrid => "Hybrid",
            YeastType.Wheat => "Wheat",
            YeastType.Wine => "Wine",
            YeastType.Champagne => "Champagne",
            _ => "Other"
        };
    }

    private static string MapToBrewfatherYeastForm(YeastForm form)
    {
        return form switch
        {
            YeastForm.Dry => "Dry",
            YeastForm.Liquid => "Liquid",
            YeastForm.Slurry => "Slurry",
            YeastForm.Culture => "Culture",
            _ => "Liquid"
        };
    }

    private static string MapToBrewfatherMiscType(MiscType type)
    {
        return type switch
        {
            MiscType.Spice => "Spice",
            MiscType.Herb => "Herb",
            MiscType.Fruit => "Fruit",
            MiscType.Flavor => "Flavoring",
            MiscType.Fining => "Fining",
            MiscType.WaterAgent => "Water Agent",
            _ => "Other"
        };
    }

    private static string MapToBrewfatherMiscUnit(InventoryUnit unit)
    {
        return unit switch
        {
            InventoryUnit.Kilograms => "kg",
            InventoryUnit.Liters => "l",
            InventoryUnit.Milliliters => "ml",
            InventoryUnit.Packages => "pkg",
            InventoryUnit.Tablets => "items",
            _ => "g"
        };
    }

    private FermentableType MapBrewfatherType(string? brewfatherType)
    {
        return brewfatherType?.ToLowerInvariant() switch
        {
            "grain" => FermentableType.Grain,
            "sugar" => FermentableType.Sugar,
            "extract" => FermentableType.LiquidExtract,
            "dry extract" => FermentableType.DryExtract,
            "adjunct" => FermentableType.Adjunct,
            _ => FermentableType.Other
        };
    }

    private HopType MapBrewfatherHopType(string? brewfatherType)
    {
        return brewfatherType?.ToLowerInvariant() switch
        {
            "pellet" => HopType.Pellet,
            "whole" or "leaf" => HopType.Whole,
            "cryo" => HopType.Cryo,
            "extract" or "co2 extract" => HopType.CO2Extract,
            _ => HopType.Pellet
        };
    }

    private YeastType MapBrewfatherYeastType(string? brewfatherType)
    {
        return brewfatherType?.ToLowerInvariant() switch
        {
            "ale" => YeastType.Ale,
            "lager" => YeastType.Lager,
            "hybrid" => YeastType.Hybrid,
            "wheat" => YeastType.Wheat,
            "wine" => YeastType.Wine,
            "champagne" => YeastType.Champagne,
            _ => YeastType.Other
        };
    }

    private YeastForm MapBrewfatherYeastForm(string? brewfatherForm)
    {
        return brewfatherForm?.ToLowerInvariant() switch
        {
            "dry" => YeastForm.Dry,
            "liquid" => YeastForm.Liquid,
            "slurry" => YeastForm.Slurry,
            "culture" => YeastForm.Culture,
            _ => YeastForm.Liquid
        };
    }

    private MiscType MapBrewfatherMiscType(string? brewfatherType)
    {
        return brewfatherType?.ToLowerInvariant() switch
        {
            "spice" => MiscType.Spice,
            "herb" => MiscType.Herb,
            "fruit" => MiscType.Fruit,
            "flavor" or "flavoring" => MiscType.Flavor,
            "fining" => MiscType.Fining,
            "water agent" or "water" => MiscType.WaterAgent,
            _ => MiscType.Other
        };
    }

    private InventoryUnit MapBrewfatherMiscUnit(string? brewfatherUnit)
    {
        return brewfatherUnit?.ToLowerInvariant() switch
        {
            "g" or "gram" or "grams" => InventoryUnit.Grams,
            "kg" or "kilogram" or "kilograms" => InventoryUnit.Kilograms,
            "ml" or "milliliter" or "milliliters" => InventoryUnit.Milliliters,
            "l" or "liter" or "liters" => InventoryUnit.Liters,
            "pkg" or "package" or "packages" => InventoryUnit.Packages,
            "tablet" or "tablets" => InventoryUnit.Tablets,
            _ => InventoryUnit.Grams
        };
    }
}
