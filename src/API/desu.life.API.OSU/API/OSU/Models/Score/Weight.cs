#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Weight
{
    [JsonPropertyName("percentage")]
    public double Percentage { get; set; }

    [JsonPropertyName("pp")]
    public double Pp { get; set; }
}