#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以
using System.Text.Json.Serialization;

namespace desu.life.API;

public partial class OSU
{
    public partial class Models
    {
        public class BaseUser
        {
            [JsonPropertyName("avatar_url")]
            public string AvatarUrl { get; set; }

            [JsonPropertyName("country_code")]
            public string CountryCode { get; set; }

            [JsonPropertyName("default_group")]
            public string DefaultGroup { get; set; }

            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("is_active")]
            public bool IsActive { get; set; }

            [JsonPropertyName("is_bot")]
            public bool IsBot { get; set; }

            [JsonPropertyName("is_deleted")]
            public bool IsDeleted { get; set; }

            [JsonPropertyName("is_online")]
            public bool IsOnline { get; set; }

            [JsonPropertyName("is_supporter")]
            public bool IsSupporter { get; set; }

            [JsonPropertyName("last_visit")]
            public DateTime LastVisit { get; set; }

            [JsonPropertyName("pm_friends_only")]
            public bool PmFriendsOnly { get; set; }

            [JsonPropertyName("profile_colour")]
            public string ProfileColour { get; set; }

            [JsonPropertyName("username")]
            public string Username { get; set; }
        }

        public class BaseUserProfile : BaseUser
        {
            [JsonPropertyName("cover_url")]
            public string CoverUrl { get; set; }

            [JsonPropertyName("discord")]
            public string Discord { get; set; }

            [JsonPropertyName("has_supported")]
            public bool HasSupported { get; set; }

            [JsonPropertyName("interests")]
            public string Interests { get; set; }

            [JsonPropertyName("join_date")]
            public DateTime JoinDate { get; set; }

            [JsonPropertyName("location")]
            public string Location { get; set; }

            [JsonPropertyName("max_blocks")]
            public int MaxBlocks { get; set; }

            [JsonPropertyName("max_friends")]
            public int MaxFriends { get; set; }

            [JsonPropertyName("occupation")]
            public string Occupation { get; set; }

            [JsonPropertyName("playmode")]
            public string PlayMode { get; set; }

            [JsonPropertyName("playstyle")]
            public List<string> PlayStyle { get; set; }

            [JsonPropertyName("post_count")]
            public int PostCount { get; set; }

            [JsonPropertyName("profile_order")]
            public List<string> ProfileOrder { get; set; }

            [JsonPropertyName("title")]
            public string Title { get; set; }

            [JsonPropertyName("title_url")]
            public string TitleUrl { get; set; }

            [JsonPropertyName("twitter")]
            public string Twitter { get; set; }

            [JsonPropertyName("website")]
            public string Website { get; set; }

            [JsonPropertyName("country")]
            public Country Country { get; set; }

            [JsonPropertyName("cover")]
            public Cover Cover { get; set; }

            [JsonPropertyName("is_restricted")]
            public bool IsRestricted { get; set; }

            [JsonPropertyName("kudosu")]
            public Kudosu Kudosu { get; set; }

            [JsonPropertyName("monthly_playcounts")]
            public List<MonthlyPlaycount> MonthlyPlaycounts { get; set; }

            [JsonPropertyName("page")]
            public UserPage Page { get; set; }

            [JsonPropertyName("user_achievements")]
            public List<UserAchievement> UserAchievements { get; set; }

            [JsonPropertyName("rank_history")]
            public RankHistory RankHistory { get; set; }
        }

        // 给OAuth用的
        public class SelfProfile : BaseUserProfile
        {
            [JsonPropertyName("statistics")]
            public UserBaseStatistics Statistics { get; set; }

            [JsonPropertyName("statistics_rulesets")]
            public Dictionary<string, UserBaseStatistics> StatisticsRulesets { get; set; }
        }

        public class UserProfile : BaseUserProfile
        {
            [JsonPropertyName("account_history")]
            public List<string> AccountHistory { get; set; }

            [JsonPropertyName("active_tournament_banner")]
            public string ActiveTournamentBanner { get; set; }

            [JsonPropertyName("active_tournament_banners")]
            public List<string> ActiveTournamentBanners { get; set; }

            [JsonPropertyName("badges")]
            public List<string> Badges { get; set; }

            [JsonPropertyName("beatmap_playcounts_count")]
            public int BeatmapPlaycountsCount { get; set; }

            [JsonPropertyName("comments_count")]
            public int CommentsCount { get; set; }

            [JsonPropertyName("favourite_beatmapset_count")]
            public int FavouriteBeatmapsetCount { get; set; }

            [JsonPropertyName("follower_count")]
            public int FollowerCount { get; set; }

            [JsonPropertyName("graveyard_beatmapset_count")]
            public int GraveyardBeatmapsetCount { get; set; }

            [JsonPropertyName("groups")]
            public List<string> Groups { get; set; }

            [JsonPropertyName("guest_beatmapset_count")]
            public int GuestBeatmapsetCount { get; set; }

            [JsonPropertyName("loved_beatmapset_count")]
            public int LovedBeatmapsetCount { get; set; }

            [JsonPropertyName("mapping_follower_count")]
            public int MappingFollowerCount { get; set; }

            [JsonPropertyName("nominated_beatmapset_count")]
            public int NominatedBeatmapsetCount { get; set; }

            [JsonPropertyName("pending_beatmapset_count")]
            public int PendingBeatmapsetCount { get; set; }

            [JsonPropertyName("previous_usernames")]
            public List<string> PreviousUsernames { get; set; }

            [JsonPropertyName("rank_highest")]
            public RankHighest RankHighest { get; set; }

            [JsonPropertyName("ranked_beatmapset_count")]
            public int RankedBeatmapsetCount { get; set; }

            [JsonPropertyName("replays_watched_counts")]
            public List<ReplaysWatchedCount> ReplaysWatchedCounts { get; set; }

            [JsonPropertyName("scores_best_count")]
            public int ScoresBestCount { get; set; }

            [JsonPropertyName("scores_first_count")]
            public int ScoresFirstCount { get; set; }

            [JsonPropertyName("scores_pinned_count")]
            public int ScoresPinnedCount { get; set; }

            [JsonPropertyName("scores_recent_count")]
            public int ScoresRecentCount { get; set; }

            [JsonPropertyName("support_level")]
            public int SupportLevel { get; set; }

            [JsonPropertyName("ranked_and_approved_beatmapset_count")]
            public int RankedAndApprovedBeatmapsetCount { get; set; }

            [JsonPropertyName("unranked_beatmapset_count")]
            public int UnrankedBeatmapsetCount { get; set; }

            [JsonPropertyName("statistics")]
            public UserStatistics Statistics { get; set; }
        }
    }
}
