#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class UserPage
{
    [JsonPropertyName("html")]
    public string Html { get; set; }

    [JsonPropertyName("raw")]
    public string Raw { get; set; }
}
