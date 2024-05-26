using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using desu.life.Utils;

namespace desu.life.Serializer;

public static class Json
{
    public static string Serialize(object? self) => JsonConvert.SerializeObject(self, Settings.Json);
    public static string Serialize(object? self, Formatting format) => JsonConvert.SerializeObject(self, format);
    public static T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, Settings.Json);
    public static JObject ToLinq(string json) => JObject.Parse(json);
}

internal static class Settings
{
    public static readonly JsonSerializerSettings Json = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Formatting = Formatting.None,
        Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}

public class JsonEnumConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        var enumType = Nullable.GetUnderlyingType(objectType) ?? objectType;
        return enumType.IsEnum;
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.Value is long)
            return Enum.ToObject(objectType, reader.Value);

        string description = reader.Value?.ToString() ?? string.Empty;

        if (description is null) return null;

        foreach (var field in objectType.GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return field.GetValue(null);
            }
        }

        throw new ArgumentException("Not found.", nameof(description));
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (string.IsNullOrEmpty(value!.ToString()))
        {
            writer.WriteValue("");
            return;
        }
        writer.WriteValue(DescriptionUtils.GetDescription(value));
    }
}