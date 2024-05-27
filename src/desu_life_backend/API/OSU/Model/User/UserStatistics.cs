#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
using System.Text.Json.Serialization;
using static desu.life.API.OSU.Models;

namespace desu.life.API;

public partial class OSU
{
    public partial class Models
    {
        public class UserBaseStatistics
        {
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

        public class UserStatistics : UserBaseStatistics
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
    }
}