#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.Responses;

public class LinkOsuResponse
{
    [JsonPropertyName("RedirectUrl")] public string RedirectUrl { get; set; }
}