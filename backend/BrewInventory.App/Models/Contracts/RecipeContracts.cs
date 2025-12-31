namespace BrewInventory.App.Models.Contracts;

public record RecipeListResponse(
    int Id,
    string Name,
    string? BrewfatherId);

public record RecipeDetailsResponse(
    int Id,
    string Name,
    string? BrewfatherId,
    List<RecipeFermentableDetail> Fermentables,
    List<RecipeHopDetail> Hops,
    List<RecipeYeastDetail> Yeasts,
    List<RecipeMiscDetail> Miscs);

public record RecipeFermentableDetail(
    int FermentableId,
    string Name,
    string Type,
    decimal Amount,
    string? Supplier,
    string? Origin,
    double Color);

public record RecipeHopDetail(
    int HopId,
    string Name,
    string Type,
    decimal Amount,
    string? Origin,
    double AlphaAcid,
    int? HarvestYear);

public record RecipeYeastDetail(
    int YeastId,
    string Name,
    string Type,
    string Form,
    decimal Amount,
    string Laboratory);

public record RecipeMiscDetail(
    int MiscId,
    string Name,
    string Type,
    string Unit,
    decimal Amount);
