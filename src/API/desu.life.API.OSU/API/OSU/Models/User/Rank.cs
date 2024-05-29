#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Rank
{
    [JsonPropertyName("country")]
    public int Country { get; set; }
}