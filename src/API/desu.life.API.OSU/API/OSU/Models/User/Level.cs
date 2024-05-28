#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Level
{
    [JsonPropertyName("current")]
    public int Current { get; set; }

    [JsonPropertyName("progress")]
    public int Progress { get; set; }
}