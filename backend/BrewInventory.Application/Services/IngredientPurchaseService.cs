using BrewInventory.Application.Contracts.IngredientPurchase;
using BrewInventory.Application.Repositories;

namespace BrewInventory.Application.Services;

public class IngredientPurchaseService(
    IRecipeRepository recipeRepository,
    IFermentableRepository fermentableRepository,
    IHopRepository hopRepository,
    IYeastRepository yeastRepository,
    IMiscRepository miscRepository,
    IExcelExporter excelExporter) : IIngredientPurchaseService
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IFermentableRepository _fermentableRepository = fermentableRepository;
    private readonly IHopRepository _hopRepository = hopRepository;
    private readonly IYeastRepository _yeastRepository = yeastRepository;
    private readonly IMiscRepository _miscRepository = miscRepository;
    private readonly IExcelExporter _excelExporter = excelExporter;

    public async Task<IngredientPurchaseResponse> CalculateIngredientNeedsAsync(ICollection<int> recipeIds, CancellationToken cancellationToken = default)
    {
        var recipes = await _recipeRepository.GetByIdsAsync(recipeIds, cancellationToken);
        if (recipes.Count == 0)
        {
            return new IngredientPurchaseResponse([], [], [], []);
        }

        var fermentableInventory = await LoadFermentableInventoryAsync(cancellationToken);
        var hopInventory = await LoadHopInventoryAsync(cancellationToken);
        var yeastInventory = await LoadYeastInventoryAsync(cancellationToken);
        var miscInventory = await LoadMiscInventoryAsync(cancellationToken);

        var fermentableNeeds = CalculateFermentableNeeds(recipes, fermentableInventory);
        var hopNeeds = CalculateHopNeeds(recipes, hopInventory);
        var yeastNeeds = CalculateYeastNeeds(recipes, yeastInventory);
        var miscNeeds = CalculateMiscNeeds(recipes, miscInventory);

        return new IngredientPurchaseResponse(fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds);
    }

    public async Task<byte[]> ExportToExcelAsync(ICollection<int> recipeIds, CancellationToken cancellationToken = default)
    {
        var recipes = await _recipeRepository.GetByIdsAsync(recipeIds, cancellationToken);
        if (recipes.Count == 0)
        {
            return [];
        }

        var fermentableInventory = await LoadFermentableInventoryAsync(cancellationToken);
        var hopInventory = await LoadHopInventoryAsync(cancellationToken);
        var yeastInventory = await LoadYeastInventoryAsync(cancellationToken);
        var miscInventory = await LoadMiscInventoryAsync(cancellationToken);

        var fermentableNeeds = CalculateFermentableNeeds(recipes, fermentableInventory);
        var hopNeeds = CalculateHopNeeds(recipes, hopInventory);
        var yeastNeeds = CalculateYeastNeeds(recipes, yeastInventory);
        var miscNeeds = CalculateMiscNeeds(recipes, miscInventory);

        return _excelExporter.GeneratePurchaseList(fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds);
    }

    private async Task<Dictionary<string, (double Amount, string? Supplier)>> LoadFermentableInventoryAsync(CancellationToken cancellationToken)
    {
        var items = await _fermentableRepository.GetAllAsync(cancellationToken);
        return items.Where(f => !string.IsNullOrWhiteSpace(f.Name))
                    .GroupBy(f => f.Name, StringComparer.InvariantCultureIgnoreCase)
                    .ToDictionary(
                        g => g.Key,
                        g => (
                            g.Sum(f => f.Amount),
                            g.First().Supplier),
                        StringComparer.InvariantCultureIgnoreCase);
    }

    private async Task<Dictionary<string, double>> LoadHopInventoryAsync(CancellationToken cancellationToken)
    {
        var items = await _hopRepository.GetAllAsync(cancellationToken);
        return items.Where(h => !string.IsNullOrWhiteSpace(h.Name))
                    .GroupBy(h => h.Name, StringComparer.InvariantCultureIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.Sum(h => h.Amount), StringComparer.InvariantCultureIgnoreCase);
    }

    private async Task<Dictionary<string, (double Amount, string? ProductId)>> LoadYeastInventoryAsync(CancellationToken cancellationToken)
    {
        var items = await _yeastRepository.GetAllAsync(cancellationToken);
        return items.Where(y => !string.IsNullOrWhiteSpace(y.Name))
                    .GroupBy(y => y.Name, StringComparer.InvariantCultureIgnoreCase)
                    .ToDictionary(g => g.Key, g => (g.Sum(y => y.Amount), g.First().ProductId), StringComparer.InvariantCultureIgnoreCase);
    }

    private async Task<Dictionary<string, double>> LoadMiscInventoryAsync(CancellationToken cancellationToken)
    {
        var items = await _miscRepository.GetAllAsync(cancellationToken);
        return items.Where(m => !string.IsNullOrWhiteSpace(m.Name))
                    .GroupBy(m => m.Name, StringComparer.InvariantCultureIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.Sum(m => m.Amount), StringComparer.InvariantCultureIgnoreCase);
    }

    private static List<IngredientNeedDetail> CalculateFermentableNeeds(IEnumerable<Domain.Entities.Recipe> recipes, Dictionary<string, (double Amount, string? Supplier)> inventory)
    {
        return recipes
            .SelectMany(r => r.RecipeFermentables)
            .GroupBy(rf => rf.Name, StringComparer.InvariantCultureIgnoreCase)
            .Select(g =>
            {
                var name = g.Key;
                var amountNeeded = g.Sum(rf => rf.Amount);
                inventory.TryGetValue(name, out var inv);
                var supplier = g.First().Supplier ?? inv.Supplier;
                return new IngredientNeedDetail(
                    name,
                    g.First().Type.ToString(),
                    amountNeeded,
                    inv.Amount,
                    Math.Max(0, amountNeeded - inv.Amount),
                    "kg",
                    supplier);
            })
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateHopNeeds(IEnumerable<Domain.Entities.Recipe> recipes, Dictionary<string, double> inventory)
    {
        return recipes
            .SelectMany(r => r.RecipeHops)
            .GroupBy(rh => rh.Name, StringComparer.InvariantCultureIgnoreCase)
            .Select(g =>
            {
                var name = g.Key;
                var amountNeeded = g.Sum(rh => rh.Amount);
                inventory.TryGetValue(name, out var amountInInventory);
                return new IngredientNeedDetail(
                    name,
                    g.First().Type.ToString(),
                    amountNeeded,
                    amountInInventory,
                    Math.Max(0, amountNeeded - amountInInventory),
                    "g");
            })
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateYeastNeeds(IEnumerable<Domain.Entities.Recipe> recipes, Dictionary<string, (double Amount, string? ProductId)> inventory)
    {
        return recipes
            .SelectMany(r => r.RecipeYeasts)
            .GroupBy(ry => ry.Name, StringComparer.InvariantCultureIgnoreCase)
            .Select(g =>
            {
                var name = g.Key;
                var amountNeeded = g.Sum(ry => ry.Amount);
                inventory.TryGetValue(name, out var inv);
                return new IngredientNeedDetail(
                    name,
                    g.First().Type.ToString(),
                    amountNeeded,
                    inv.Amount,
                    Math.Max(0, amountNeeded - inv.Amount),
                    g.First().Unit,
                    ProductId: inv.ProductId);
            })
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateMiscNeeds(IEnumerable<Domain.Entities.Recipe> recipes, Dictionary<string, double> inventory)
    {
        return recipes
            .SelectMany(r => r.RecipeMiscs)
            .GroupBy(rm => rm.Name, StringComparer.InvariantCultureIgnoreCase)
            .Select(g =>
            {
                var name = g.Key;
                var amountNeeded = g.Sum(rm => rm.Amount);
                inventory.TryGetValue(name, out var amountInInventory);
                return new IngredientNeedDetail(
                    name,
                    g.First().Type.ToString(),
                    amountNeeded,
                    amountInInventory,
                    Math.Max(0, amountNeeded - amountInInventory),
                    g.First().Unit);
            })
            .ToList();
    }
}
