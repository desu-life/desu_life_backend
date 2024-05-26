using desu.life.Utils;
using System.Text.Json;
using System.Globalization;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace desu.life.Serializer;

public static class Json
{
    public static string Serialize(object? self) => JsonSerializer.Serialize(self, Settings.Json);
    public static string Serialize<T>(T self, JsonSerializerOptions format) => JsonSerializer.Serialize(self, format);
    public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, Settings.Json);
}

internal static class Settings
{
    public static readonly JsonSerializerOptions Json = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new JsonDescriptionConverter()
        }
    };
}

public class JsonDescriptionConverter : JsonConverter<Enum>
{
    public override Enum? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? description = reader.GetString();
        if (description == null) return null;

        foreach (var field in typeToConvert.GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description == description)
                    return field.GetValue(null) as Enum;
            }
            else if (field.Name.Equals(description, StringComparison.InvariantCultureIgnoreCase))
            {
                return field.GetValue(null) as Enum;
            }
        }

        throw new JsonException($"Unable to convert '{description}' to enum '{typeToConvert}'.");
    }

    public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
    {
        var description = DescriptionUtils.GetDescription(value);
        writer.WriteStringValue(description ?? value.ToString());
    }
}