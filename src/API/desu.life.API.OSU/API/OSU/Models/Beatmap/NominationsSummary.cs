#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class NominationsSummary
{
    [JsonPropertyName("current")]
    public int Current { get; set; }

    [JsonPropertyName("required")]
    public int Required { get; set; }
}