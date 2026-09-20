using System.Text.Json.Serialization;

namespace BrewInventory.Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum YeastType
{
    Ale,
    Lager,
    Hybrid,
    Wheat,
    Wine,
    Champagne,
    Other
}
