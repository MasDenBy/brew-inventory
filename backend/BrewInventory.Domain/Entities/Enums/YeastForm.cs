using System.Text.Json.Serialization;

namespace BrewInventory.Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum YeastForm
{
    Dry,
    Liquid,
    Slant,
    Culture,
    Slurry
}
