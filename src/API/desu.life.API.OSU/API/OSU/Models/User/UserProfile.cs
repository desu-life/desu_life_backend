using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class UserBase
{
    [JsonPropertyName("avatar_url")]
    public required string AvatarUrl { get; set; }

    [JsonPropertyName("country_code")]
    public required string CountryCode { get; set; }

    [JsonPropertyName("default_group")]
    public required string DefaultGroup { get; set; }

    [JsonPropertyName("id")]
    public required int Id { get; set; }

    [JsonPropertyName("is_active")]
    public required bool IsActive { get; set; }

    [JsonPropertyName("is_bot")]
    public required bool IsBot { get; set; }

    [JsonPropertyName("is_deleted")]
    public required bool IsDeleted { get; set; }

    [JsonPropertyName("is_online")]
    public required bool IsOnline { get; set; }

    [JsonPropertyName("is_supporter")]
    public required bool IsSupporter { get; set; }

    [JsonPropertyName("last_visit")]
    public required DateTime LastVisit { get; set; }

    [JsonPropertyName("pm_friends_only")]
    public required bool PmFriendsOnly { get; set; }

    [JsonPropertyName("profile_colour")]
    public required string ProfileColour { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    /// <summary>
    /// 注：GetUserScores无此字段
    /// </summary>
    [JsonPropertyName("country")]
    public Country? Country { get; set; }

    /// <summary>
    /// 注：GetUserScores无此字段
    /// </summary>
    [JsonPropertyName("cover")]
    public Cover? Cover { get; set; }
}

public class UserProfile : UserBase
{
    [JsonPropertyName("cover_url")]
    public required string CoverUrl { get; set; }

    [JsonPropertyName("discord")]
    public required string Discord { get; set; }

    [JsonPropertyName("has_supported")]
    public required bool HasSupported { get; set; }

    [JsonPropertyName("interests")]
    public required string Interests { get; set; }

    [JsonPropertyName("join_date")]
    public required DateTime JoinDate { get; set; }

    [JsonPropertyName("location")]
    public required string Location { get; set; }

    [JsonPropertyName("max_blocks")]
    public required int MaxBlocks { get; set; }

    [JsonPropertyName("max_friends")]
    public required int MaxFriends { get; set; }

    [JsonPropertyName("occupation")]
    public required string Occupation { get; set; }

    [JsonPropertyName("playmode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required GameMode PlayMode { get; set; }

    [JsonPropertyName("playstyle")]
    public required List<string> PlayStyle { get; set; }

    [JsonPropertyName("post_count")]
    public required int PostCount { get; set; }

    [JsonPropertyName("profile_order")]
    public required List<string> ProfileOrder { get; set; }

    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("title_url")]
    public required string TitleUrl { get; set; }

    [JsonPropertyName("twitter")]
    public required string Twitter { get; set; }

    [JsonPropertyName("website")]
    public required string Website { get; set; }

    [JsonPropertyName("kudosu")]
    public required Kudosu Kudosu { get; set; }

    [JsonPropertyName("account_history")]
    public required List<string> AccountHistory { get; set; }

    [JsonPropertyName("active_tournament_banner")]
    public required string ActiveTournamentBanner { get; set; }

    [JsonPropertyName("active_tournament_banners")]
    public required List<string> ActiveTournamentBanners { get; set; }

    [JsonPropertyName("badges")]
    public required List<string> Badges { get; set; }

    [JsonPropertyName("beatmap_playcounts_count")]
    public required int BeatmapPlaycountsCount { get; set; }

    [JsonPropertyName("comments_count")]
    public required int CommentsCount { get; set; }

    [JsonPropertyName("favourite_beatmapset_count")]
    public required int FavouriteBeatmapsetCount { get; set; }

    [JsonPropertyName("follower_count")]
    public required int FollowerCount { get; set; }

    [JsonPropertyName("graveyard_beatmapset_count")]
    public required int GraveyardBeatmapsetCount { get; set; }

    [JsonPropertyName("groups")]
    public required List<string> Groups { get; set; }

    [JsonPropertyName("guest_beatmapset_count")]
    public required int GuestBeatmapsetCount { get; set; }

    [JsonPropertyName("loved_beatmapset_count")]
    public required int LovedBeatmapsetCount { get; set; }

    [JsonPropertyName("mapping_follower_count")]
    public required int MappingFollowerCount { get; set; }

    [JsonPropertyName("monthly_playcounts")]
    public required List<MonthlyPlaycount> MonthlyPlaycounts { get; set; }

    [JsonPropertyName("nominated_beatmapset_count")]
    public required int NominatedBeatmapsetCount { get; set; }

    [JsonPropertyName("page")]
    public required UserPage Page { get; set; }

    [JsonPropertyName("pending_beatmapset_count")]
    public required int PendingBeatmapsetCount { get; set; }

    [JsonPropertyName("previous_usernames")]
    public required List<string> PreviousUsernames { get; set; }

    [JsonPropertyName("rank_highest")]
    public required RankHighest RankHighest { get; set; }

    [JsonPropertyName("ranked_beatmapset_count")]
    public required int RankedBeatmapsetCount { get; set; }

    [JsonPropertyName("replays_watched_counts")]
    public required List<ReplaysWatchedCount> ReplaysWatchedCounts { get; set; }

    [JsonPropertyName("scores_best_count")]
    public required int ScoresBestCount { get; set; }

    [JsonPropertyName("scores_first_count")]
    public required int ScoresFirstCount { get; set; }

    [JsonPropertyName("scores_pinned_count")]
    public required int ScoresPinnedCount { get; set; }

    [JsonPropertyName("scores_recent_count")]
    public required int ScoresRecentCount { get; set; }

    [JsonPropertyName("statistics")]
    public required UserStatistics Statistics { get; set; }

    [JsonPropertyName("support_level")]
    public required int SupportLevel { get; set; }

    [JsonPropertyName("user_achievements")]
    public required List<UserAchievement> UserAchievements { get; set; }

    [JsonPropertyName("rank_history")]
    public required RankHistory RankHistory { get; set; }

    /// <summary>
    /// caonima ppy
    /// </summary>
    [JsonPropertyName("rankHistory")]
    public required RankHistory RankHistoryOld { get; set; }

    [JsonPropertyName("ranked_and_approved_beatmapset_count")]
    public required int RankedAndApprovedBeatmapsetCount { get; set; }

    [JsonPropertyName("unranked_beatmapset_count")]
    public required int UnrankedBeatmapsetCount { get; set; }
}

public class OAuthUserProfile : UserProfile
{
    [JsonPropertyName("is_restricted")]
    public required bool IsRestricted { get; set; }

    [JsonPropertyName("statistics_rulesets")]
    public required Dictionary<string, UserStatisticsBase> StatisticsRulesets { get; set; }
}