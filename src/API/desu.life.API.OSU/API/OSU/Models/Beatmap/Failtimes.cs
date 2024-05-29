#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Failtimes
{
    [JsonPropertyName("fail")]
    public List<int> Fail { get; set; }

    [JsonPropertyName("exit")]
    public List<int> Exit { get; set; }
}