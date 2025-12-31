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

            foreach (var bfRecipe in brewfatherRecipes)
            {
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
                "Recipes synchronization completed. Added: {Added}, Updated: {Updated}, Deleted: {Deleted}",
                addedCount, updatedCount, deletedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during recipes synchronization");
            throw;
        }
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

    private FermentableType MapBrewfatherType(string brewfatherType)
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

    private HopType MapBrewfatherHopType(string brewfatherType)
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

    private YeastType MapBrewfatherYeastType(string brewfatherType)
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

    private MiscType MapBrewfatherMiscType(string brewfatherType)
    {
        return brewfatherType?.ToLowerInvariant() switch
        {
            "spice" => MiscType.Spice,
            "herb" => MiscType.Herb,
            "fruit" => MiscType.Fruit,
            "flavor" or "flavoring" => MiscType.Flavor,
            "water agent" or "water" => MiscType.WaterAgent,
            _ => MiscType.Other
        };
    }
}
