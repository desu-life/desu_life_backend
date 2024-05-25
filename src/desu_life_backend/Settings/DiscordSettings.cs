namespace desu.life.Settings;

public class DiscordSettings
{
    public required string ClientID { get; init; }
    public required string ClientSecret { get; init; }
    public required string RedirectUri { get; init; }
}