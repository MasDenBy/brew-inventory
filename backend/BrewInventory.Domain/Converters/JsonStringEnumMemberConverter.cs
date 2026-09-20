using System.Globalization;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BrewInventory.Domain.Converters;

public class JsonStringEnumMemberConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    private readonly Dictionary<TEnum, string> _enumToString = [];
    private readonly Dictionary<string, TEnum> _stringToEnum = [];
    private readonly Dictionary<int, TEnum> _numberToEnum = [];

    public JsonStringEnumMemberConverter()
    {
        var type = typeof(TEnum);

        foreach (var value in Enum.GetValues<TEnum>())
        {
            var enumMember = type.GetMember(value.ToString())[0];
            var attr = enumMember.GetCustomAttributes(typeof(EnumMemberAttribute), false)
              .Cast<EnumMemberAttribute>()
              .FirstOrDefault();

            _stringToEnum.Add(value.ToString(), value);

            var strValue = type.GetField("value__")?.GetValue(value);
            var numValue = Convert.ToInt32(strValue, CultureInfo.InvariantCulture);

            if (attr?.Value != null)
            {
                _enumToString.Add(value, attr.Value);
                _stringToEnum.Add(attr.Value, value);
                _numberToEnum.Add(numValue, value);
            }
            else
            {
                _enumToString.Add(value, value.ToString());
            }
        }
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var type = reader.TokenType;
        if (type == JsonTokenType.String)
        {
            var stringValue = reader.GetString();

            if(stringValue == null)
            {
                return default;
            }

            if (_stringToEnum.TryGetValue(stringValue, out var enumValue))
            {
                return enumValue;
            }
        }
        else if (type == JsonTokenType.Number)
        {
            var numValue = reader.GetInt32();
            _numberToEnum.TryGetValue(numValue, out var enumValue);
            return enumValue;
        }

        return default;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStringValue(_enumToString[value]);
    }
}
