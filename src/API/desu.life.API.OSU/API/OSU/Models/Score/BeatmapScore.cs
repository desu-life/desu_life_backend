using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public struct BeatmapUserScore
{
    [JsonPropertyName("position")]
    public required int Pos { get; set; }

    [JsonPropertyName("score")]
    public required Score Score { get; set; }

}

public struct BeatmapScores
{
    [JsonPropertyName("scores")]
    public required List<Score> Scores { get; set; }
}