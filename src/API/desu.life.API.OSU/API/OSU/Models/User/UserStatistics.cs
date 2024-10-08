﻿#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public abstract class UserStatisticsBase
{
    public UserStatisticsBase(){}
    [JsonPropertyName("level")]
    public Level Level { get; set; }

    [JsonPropertyName("global_rank")]
    public int? GlobalRank { get; set; }

    [JsonPropertyName("pp")]
    public double Pp { get; set; }

    [JsonPropertyName("ranked_score")]
    public long RankedScore { get; set; }

    [JsonPropertyName("hit_accuracy")]
    public double HitAccuracy { get; set; }

    [JsonPropertyName("play_count")]
    public int PlayCount { get; set; }

    [JsonPropertyName("play_time")]
    public long PlayTime { get; set; }

    [JsonPropertyName("total_score")]
    public long TotalScore { get; set; }

    [JsonPropertyName("total_hits")]
    public int TotalHits { get; set; }

    [JsonPropertyName("maximum_combo")]
    public int MaximumCombo { get; set; }

    [JsonPropertyName("replays_watched_by_others")]
    public int ReplaysWatchedByOthers { get; set; }

    [JsonPropertyName("is_ranked")]
    public bool IsRanked { get; set; }

    [JsonPropertyName("grade_counts")]
    public GradeCounts GradeCounts { get; set; }
}

public class UserStatistics : UserStatisticsBase
{
    [JsonPropertyName("count_100")]
    public int Count100 { get; set; }

    [JsonPropertyName("count_300")]
    public int Count300 { get; set; }

    [JsonPropertyName("count_50")]
    public int Count50 { get; set; }

    [JsonPropertyName("count_miss")]
    public int CountMiss { get; set; }

    [JsonPropertyName("global_rank_exp")]
    public int? GlobalRankExp { get; set; }

    [JsonPropertyName("pp_exp")]
    public double PpExp { get; set; }

    [JsonPropertyName("country_rank")]
    public int CountryRank { get; set; }

    [JsonPropertyName("rank")]
    public Rank Rank { get; set; }
}