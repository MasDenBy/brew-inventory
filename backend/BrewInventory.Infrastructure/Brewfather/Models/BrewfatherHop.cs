using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherHop
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;
    public string Name { get; init; } = null!;
    public double Alpha { get; init; }
    public HopUse Use { get; init; }
    public HopType Type { get; init; }
    public string? Origin { get; init; }
    public string? Usage { get; init; }
    public double? Inventory { get; init; }
    public int? Year { get; init; }
}
