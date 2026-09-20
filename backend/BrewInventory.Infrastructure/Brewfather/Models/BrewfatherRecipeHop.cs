using System.Text.Json.Serialization;
using BrewInventory.Domain.Entities.Enums;

namespace BrewInventory.Infrastructure.Brewfather.Models;

public record BrewfatherRecipeHop
{
    [JsonPropertyName("_id")]
    public string Id { get; init; } = null!;
    public double Amount {get; init;}
    public string Name { get; init; } = null!;
    public double? Alpha {get; init;}
    public HopType Type {get; init;}
    public string? Origin {get; init;}
    public HopUse Use {get; init;}
    public double? Time {get; init;}
}
