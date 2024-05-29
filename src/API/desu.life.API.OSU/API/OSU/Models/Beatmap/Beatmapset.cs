#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.API.OSU.Models;

public class Beatmapset
{
    [JsonPropertyName("artist")]
    public string Artist { get; set; }

    [JsonPropertyName("artist_unicode")]
    public string ArtistUnicode { get; set; }

    [JsonPropertyName("covers")]
    public BeatmapCovers Covers { get; set; }

    [JsonPropertyName("creator")]
    public string Creator { get; set; }

    [JsonPropertyName("favourite_count")]
    public int FavouriteCount { get; set; }

    [JsonPropertyName("hype")]
    public int? Hype { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("nsfw")]
    public bool Nsfw { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("play_count")]
    public int PlayCount { get; set; }

    [JsonPropertyName("preview_url")]
    public string PreviewUrl { get; set; }

    [JsonPropertyName("source")]
    public string Source { get; set; }

    [JsonPropertyName("spotlight")]
    public bool Spotlight { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("title_unicode")]
    public string TitleUnicode { get; set; }

    [JsonPropertyName("track_id")]
    public int? TrackId { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("video")]
    public bool Video { get; set; }

    [JsonPropertyName("bpm")]
    public double Bpm { get; set; }

    [JsonPropertyName("can_be_hyped")]
    public bool CanBeHyped { get; set; }

    [JsonPropertyName("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [JsonPropertyName("discussion_enabled")]
    public bool DiscussionEnabled { get; set; }

    [JsonPropertyName("discussion_locked")]
    public bool DiscussionLocked { get; set; }

    [JsonPropertyName("is_scoreable")]
    public bool IsScoreable { get; set; }

    [JsonPropertyName("last_updated")]
    public DateTime LastUpdated { get; set; }

    [JsonPropertyName("legacy_thread_url")]
    public string LegacyThreadUrl { get; set; }

    [JsonPropertyName("nominations_summary")]
    public NominationsSummary NominationsSummary { get; set; }

    [JsonPropertyName("ranked")]
    public int Ranked { get; set; }

    [JsonPropertyName("ranked_date")]
    public DateTime RankedDate { get; set; }

    [JsonPropertyName("storyboard")]
    public bool Storyboard { get; set; }

    [JsonPropertyName("submitted_date")]
    public DateTime SubmittedDate { get; set; }

    [JsonPropertyName("tags")]
    public string Tags { get; set; }

    [JsonPropertyName("availability")]
    public Availability Availability { get; set; }

    [JsonPropertyName("ratings")]
    public List<int> Ratings { get; set; }
}