#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class GradeCounts
{
    [JsonPropertyName("ss")]
    public int Ss { get; set; }

    [JsonPropertyName("ssh")]
    public int Ssh { get; set; }

    [JsonPropertyName("s")]
    public int S { get; set; }

    [JsonPropertyName("sh")]
    public int Sh { get; set; }

    [JsonPropertyName("a")]
    public int A { get; set; }
}