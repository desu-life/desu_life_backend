#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class MonthlyPlaycount
{
    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }
}