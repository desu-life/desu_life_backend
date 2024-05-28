#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
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