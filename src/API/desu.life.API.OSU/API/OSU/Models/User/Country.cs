#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Country
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
