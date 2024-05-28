#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class UserAchievement
{
    [JsonPropertyName("achieved_at")]
    public DateTime AchievedAt { get; set; }

    [JsonPropertyName("achievement_id")]
    public int AchievementId { get; set; }
}