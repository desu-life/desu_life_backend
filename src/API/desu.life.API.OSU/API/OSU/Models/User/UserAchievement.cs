#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class UserAchievement
{
    [JsonPropertyName("achieved_at")]
    public DateTime AchievedAt { get; set; }

    [JsonPropertyName("achievement_id")]
    public int AchievementId { get; set; }
}