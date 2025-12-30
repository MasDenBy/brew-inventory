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
