using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherMisc
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public MiscType Type { get; init; }
    public string Use { get; init; } = null!;
    public string Unit { get; init; } = null!;
    public double? Inventory { get; init; }
}
