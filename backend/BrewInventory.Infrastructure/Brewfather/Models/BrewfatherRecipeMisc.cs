using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeMisc
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;
    public double Amount { get; init; }
    public string Name { get; init; } = null!;
    public MiscType Type { get; init; }
    public string? Unit { get; init; }
    public string? Use { get; init; }
    public double? Time { get; init; }
}
