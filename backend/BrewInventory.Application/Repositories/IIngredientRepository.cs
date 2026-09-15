using BrewInventory.Domain.Entities;

namespace BrewInventory.Application.Repositories;

public interface IIngredientRepository
{
    Task<List<Fermentable>> GetAllFermentablesAsync(CancellationToken cancellationToken = default);
    Task<Fermentable?> GetFermentableByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Fermentable> AddFermentableAsync(Fermentable fermentable, CancellationToken cancellationToken = default);
    Task UpdateFermentableAsync(Fermentable fermentable, CancellationToken cancellationToken = default);
    Task DeleteFermentableAsync(Fermentable fermentable, CancellationToken cancellationToken = default);

    Task<List<Hop>> GetAllHopsAsync(CancellationToken cancellationToken = default);
    Task<Hop?> GetHopByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Hop> AddHopAsync(Hop hop, CancellationToken cancellationToken = default);
    Task UpdateHopAsync(Hop hop, CancellationToken cancellationToken = default);
    Task DeleteHopAsync(Hop hop, CancellationToken cancellationToken = default);

    Task<List<Yeast>> GetAllYeastsAsync(CancellationToken cancellationToken = default);
    Task<Yeast?> GetYeastByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Yeast> AddYeastAsync(Yeast yeast, CancellationToken cancellationToken = default);
    Task UpdateYeastAsync(Yeast yeast, CancellationToken cancellationToken = default);
    Task DeleteYeastAsync(Yeast yeast, CancellationToken cancellationToken = default);

    Task<List<Misc>> GetAllMiscsAsync(CancellationToken cancellationToken = default);
    Task<Misc?> GetMiscByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Misc> AddMiscAsync(Misc misc, CancellationToken cancellationToken = default);
    Task UpdateMiscAsync(Misc misc, CancellationToken cancellationToken = default);
    Task DeleteMiscAsync(Misc misc, CancellationToken cancellationToken = default);

    Task<bool> ExistsFermentableAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsHopAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsYeastAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsMiscAsync(int id, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
