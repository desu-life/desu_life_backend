#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Beatmap
{
    [JsonPropertyName("beatmapset_id")]
    public int BeatmapsetId { get; set; }

    [JsonPropertyName("difficulty_rating")]
    public double DifficultyRating { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("mode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public GameMode Mode { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("total_length")]
    public int TotalLength { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("accuracy")]
    public double Accuracy { get; set; }

    [JsonPropertyName("ar")]
    public double Ar { get; set; }

    [JsonPropertyName("bpm")]
    public double Bpm { get; set; }

    [JsonPropertyName("convert")]
    public bool Convert { get; set; }

    [JsonPropertyName("count_circles")]
    public int CountCircles { get; set; }

    [JsonPropertyName("count_sliders")]
    public int CountSliders { get; set; }

    [JsonPropertyName("count_spinners")]
    public int CountSpinners { get; set; }

    [JsonPropertyName("cs")]
    public double Cs { get; set; }

    [JsonPropertyName("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [JsonPropertyName("drain")]
    public double Drain { get; set; }

    [JsonPropertyName("hit_length")]
    public int HitLength { get; set; }

    [JsonPropertyName("is_scoreable")]
    public bool IsScoreable { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }

    [JsonPropertyName("mode_int")]
    public int ModeInt { get; set; }

    [JsonPropertyName("passcount")]
    public int Passcount { get; set; }

    [JsonPropertyName("playcount")]
    public int Playcount { get; set; }

    [JsonPropertyName("ranked")]
    public int Ranked { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("checksum")]
    public string Checksum { get; set; }

    [JsonPropertyName("beatmapset")]
    public Beatmapset Beatmapset { get; set; }

    [JsonPropertyName("failtimes")]
    public Failtimes Failtimes { get; set; }

    [JsonPropertyName("max_combo")]
    public int MaxCombo { get; set; }
}