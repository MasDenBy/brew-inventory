using System.Text.Json.Serialization;

namespace BrewInventory.App.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum IngredientType : int
{
    Fermentable,
    Hop,
    Yeast,
    Misc
}
