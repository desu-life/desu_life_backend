#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
using System.Text.Json.Serialization;

namespace desu.life.API;

public partial class OSU
{
    public partial class Models
    {
        public class BeatmapStatistics
        {
            [JsonPropertyName("count_100")]
            public int Count100 { get; set; }

            [JsonPropertyName("count_300")]
            public int Count300 { get; set; }

            [JsonPropertyName("count_50")]
            public int Count50 { get; set; }

            [JsonPropertyName("count_geki")]
            public int CountGeki { get; set; }

            [JsonPropertyName("count_katu")]
            public int CountKatu { get; set; }

            [JsonPropertyName("count_miss")]
            public int CountMiss { get; set; }
        }
    }
}