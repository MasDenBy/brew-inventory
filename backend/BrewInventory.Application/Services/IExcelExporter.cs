using BrewInventory.Application.Contracts.IngredientPurchase;

namespace BrewInventory.Application.Services;

public interface IExcelExporter
{
    byte[] GeneratePurchaseList(
        ICollection<IngredientNeedDetail> fermentables,
        ICollection<IngredientNeedDetail> hops,
        ICollection<IngredientNeedDetail> yeasts,
        ICollection<IngredientNeedDetail> miscs);
}
