using BrewInventory.Application.Contracts.IngredientPurchase;
using BrewInventory.Application.Repositories;
using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Services;

public class IngredientPurchaseService(IRecipeRepository recipeRepository, IExcelExporter excelExporter) : IIngredientPurchaseService
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IExcelExporter _excelExporter = excelExporter;

    public async Task<IngredientPurchaseResponse> CalculateIngredientNeedsAsync(ICollection<int> recipeIds, CancellationToken cancellationToken = default)
    {
        var recipes = await _recipeRepository.GetByIdsAsync(recipeIds, cancellationToken);
        if (recipes.Count == 0)
        {
            return new IngredientPurchaseResponse([], [], [], []);
        }

        var fermentableNeeds = CalculateFermentableNeeds(recipes);
        var hopNeeds = CalculateHopNeeds(recipes);
        var yeastNeeds = CalculateYeastNeeds(recipes);
        var miscNeeds = CalculateMiscNeeds(recipes);

        return new IngredientPurchaseResponse(fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds);
    }

    public async Task<byte[]> ExportToExcelAsync(ICollection<int> recipeIds, CancellationToken cancellationToken = default)
    {
        var recipes = await _recipeRepository.GetByIdsAsync(recipeIds, cancellationToken);
        if (recipes.Count == 0)
        {
            return [];
        }

        var fermentableNeeds = CalculateFermentableNeeds(recipes);
        var hopNeeds = CalculateHopNeeds(recipes);
        var yeastNeeds = CalculateYeastNeeds(recipes);
        var miscNeeds = CalculateMiscNeeds(recipes);

        return _excelExporter.GeneratePurchaseList(fermentableNeeds, hopNeeds, yeastNeeds, miscNeeds);
    }

    private static List<IngredientNeedDetail> CalculateFermentableNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeFermentables)
            .GroupBy(rf => rf.FermentableId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Fermentable = g.First().Fermentable,
                TotalNeeded = g.Sum(rf => rf.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Fermentable.Name,
                item.Fermentable.Type.ToString(),
                item.TotalNeeded,
                item.Fermentable.Amount,
                Math.Max(0, item.TotalNeeded - item.Fermentable.Amount),
                "kg"))
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateHopNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeHops)
            .GroupBy(rh => rh.HopId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Hop = g.First().Hop,
                TotalNeeded = g.Sum(rh => rh.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Hop.Name,
                item.Hop.Type.ToString(),
                item.TotalNeeded,
                item.Hop.Amount,
                Math.Max(0, item.TotalNeeded - item.Hop.Amount),
                "g"))
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateYeastNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeYeasts)
            .GroupBy(ry => ry.YeastId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Yeast = g.First().Yeast,
                TotalNeeded = g.Sum(ry => ry.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Yeast.Name,
                item.Yeast.Type.ToString(),
                item.TotalNeeded,
                item.Yeast.Amount,
                Math.Max(0, item.TotalNeeded - item.Yeast.Amount),
                item.Yeast.Form.ToString()))
            .ToList();
    }

    private static List<IngredientNeedDetail> CalculateMiscNeeds(List<Recipe> recipes)
    {
        return recipes
            .SelectMany(r => r.RecipeMiscs)
            .GroupBy(rm => rm.MiscId)
            .Select(g => new
            {
                IngredientId = g.Key,
                Misc = g.First().Misc,
                TotalNeeded = g.Sum(rm => rm.Amount)
            })
            .Select(item => new IngredientNeedDetail(
                item.IngredientId,
                item.Misc.Name,
                item.Misc.Type.ToString(),
                item.TotalNeeded,
                item.Misc.Amount,
                Math.Max(0, item.TotalNeeded - item.Misc.Amount),
                item.Misc.Unit.ToString()))
            .ToList();
    }
}
