using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherYeast
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;

    public string Name { get; init; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public YeastType Type { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public YeastForm Form { get; init; }
    public string Unit { get; init; } = null!;
    public string? Laboratory { get; init; }
    public string? ProductId { get; init; }
    public double Inventory { get; init; }
}
