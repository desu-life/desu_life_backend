#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class CurrentUserAttributes
{
    [JsonPropertyName("pin")]
    public string Pin { get; set; }
}