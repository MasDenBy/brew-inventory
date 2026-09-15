using System.Text.Json.Serialization;

namespace BrewInventory.Domain.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IngredientType
{
    Fermentable,
    Hop,
    Yeast,
    Misc
}
