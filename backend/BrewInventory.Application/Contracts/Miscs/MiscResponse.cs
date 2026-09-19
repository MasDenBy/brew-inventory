using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Application.Contracts.Miscs;

public record MiscResponse(
    int Id,
    string Name,
    double Amount,
    string Unit,
    MiscType Type,
    string Use,
    string? BrewfatherId
);
