#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public abstract class ScoreBase
{
    [JsonPropertyName("accuracy")]
    public double Accuracy { get; set; }

    [JsonPropertyName("best_id")]
    public long? BestId { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("max_combo")]
    public int MaxCombo { get; set; }

    [JsonPropertyName("mode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameMode Mode { get; set; }

    [JsonPropertyName("mode_int")]
    [JsonConverter(typeof(JsonNumberEnumConverter<GameMode>))]
    public GameMode ModeInt { get; set; }

    [JsonPropertyName("mods")]
    public List<string> Mods { get; set; }

    [JsonPropertyName("passed")]
    public bool Passed { get; set; }

    [JsonPropertyName("perfect")]
    public bool Perfect { get; set; }

    [JsonPropertyName("pp")]
    public double Pp { get; set; }

    [JsonPropertyName("rank")]
    public string Rank { get; set; }

    [JsonPropertyName("replay")]
    public bool Replay { get; set; }

    [JsonPropertyName("score")]
    public long Score { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }
}

public class Score : ScoreBase
{
    [JsonPropertyName("statistics")]
    public ScoreStatistics Statistics { get; set; }

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

    public double CalcAccuracy => Statistics.Accuracy(Mode);
    public double TotalHits => Statistics.TotalHits(Mode);
}