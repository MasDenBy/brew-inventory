using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeFermentable
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;
    public double Amount { get; init; }
    public string Name { get; init; } = null!;
    public FermentableType Type { get; init; }
    public string? Supplier { get; init; }
    public string? Origin { get; init; }
    public double? Color { get; init; }
    public double? Potential { get; init; }
}
