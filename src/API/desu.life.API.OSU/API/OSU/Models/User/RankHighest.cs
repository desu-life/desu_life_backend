#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class RankHighest
{
    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}