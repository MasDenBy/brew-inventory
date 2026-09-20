using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherFermentable
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;

    public string Name { get; init; } = null!;
    public FermentableType Type { get; init; }
    public string? GrainCategory { get; init; }
    public double? Percentage { get; init; }
    public double Color { get; init; }
    public double Lovibond { get; init; }
    public string Origin { get; init; } = null!;
    public string? Supplier { get; init; }
    public double? Inventory { get; init; }
}
