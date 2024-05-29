#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Kudosu
{
    [JsonPropertyName("available")]
    public int Available { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}