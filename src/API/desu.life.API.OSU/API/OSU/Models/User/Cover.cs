#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Cover
{
    [JsonPropertyName("custom_url")]
    public string CustomUrl { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("id")]
    public int? Id { get; set; }
}