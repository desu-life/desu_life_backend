#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Level
{
    [JsonPropertyName("current")]
    public int Current { get; set; }

    [JsonPropertyName("progress")]
    public int Progress { get; set; }
}