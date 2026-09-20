using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeYeast
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;
    public double Amount { get; init; }
    public string Name { get; init; } = null!;
    public string? Laboratory { get; init; }
    public YeastType Type { get; init; }
    public YeastForm Form { get; init; }
    public double? Attenuation { get; init; }
    public string? Unit { get; init; }
}
