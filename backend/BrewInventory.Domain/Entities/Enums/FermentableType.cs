using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using BrewInventory.Domain.Converters;

namespace BrewInventory.Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumMemberConverter<FermentableType>))]
public enum FermentableType
{
    Grain,
    Sugar,

    [EnumMember(Value = "Liquid Extract")]
    LiquidExtract,

    [EnumMember(Value = "Dry Extract")]
    DryExtract,
    Adjunct,
    Other
}
