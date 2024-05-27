#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
using System.Text.Json.Serialization;

namespace desu.life.API;

public partial class OSU
{
    public partial class Models
    {
        public class GradeCounts
        {
            [JsonPropertyName("ss")]
            public int Ss { get; set; }

            [JsonPropertyName("ssh")]
            public int Ssh { get; set; }

            [JsonPropertyName("s")]
            public int S { get; set; }

            [JsonPropertyName("sh")]
            public int Sh { get; set; }

            [JsonPropertyName("a")]
            public int A { get; set; }
        }
    }
}