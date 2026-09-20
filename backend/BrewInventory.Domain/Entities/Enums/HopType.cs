using System.Text.Json.Serialization;

namespace BrewInventory.Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HopType
{
    Pellet,
    Whole,
    Cryo,
    Plug,
    ISOExtract,
    CO2Extract,
}
