using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BrewInventory.Domain.Converters;

namespace BrewInventory.Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumMemberConverter<HopUse>))]
public enum HopUse
{
    Boil,

    [EnumMember(Value = "Dry Hop")]
    DryHop,
    Mash,
    [EnumMember(Value = "First Wort")]
    FirstWort,
    Aroma,
}
