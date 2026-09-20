using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BrewInventory.Domain.Converters;

namespace BrewInventory.Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumMemberConverter<MiscType>))]
public enum MiscType
{
    Spice,
    Fining,

    [EnumMember(Value = "Water Agent")]
    WaterAgent,
    Herb,
    Flavor,
    Other,
}
