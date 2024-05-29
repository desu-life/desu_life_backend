#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Availability
{
    [JsonPropertyName("download_disabled")]
    public bool DownloadDisabled { get; set; }

    [JsonPropertyName("more_information")]
    public string MoreInformation { get; set; }
}