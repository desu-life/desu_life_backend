#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class ReplaysWatchedCount
{
    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}