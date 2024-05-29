using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class BeatmapScoreExtensions
{
    [JsonPropertyName("position")]
    public int? Position { get; set; }

    [JsonPropertyName("score")]
    public BeatmapScore? Score { get; set; }

    [JsonPropertyName("scores")]
    public List<BeatmapScore>? Scores { get; set; }
}