#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class UserScore : ScoreBase
{
    [JsonPropertyName("statistics")]
    public BeatmapStatistics Statistics { get; set; }

    [JsonPropertyName("current_user_attributes")]
    public CurrentUserAttributes CurrentUserAttributes { get; set; }

    [JsonPropertyName("beatmap")]
    public Beatmap Beatmap { get; set; }

    [JsonPropertyName("beatmapset")]
    public Beatmapset Beatmapset { get; set; }

    [JsonPropertyName("user")]
    public UserBase User { get; set; }

    [JsonPropertyName("weight")]
    public Weight Weight { get; set; }
}