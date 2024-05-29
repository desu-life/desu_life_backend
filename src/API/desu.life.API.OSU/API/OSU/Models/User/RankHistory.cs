#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class RankHistory
{
    [JsonPropertyName("mode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameMode Mode { get; set; }

    [JsonPropertyName("data")]
    public List<int> Data { get; set; }
}