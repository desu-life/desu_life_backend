namespace desu.life.Services.User;

public interface IUserService
{
    Task<TokenResult> RegisterOrLogin(string username, string osuId);
    Task FillLoginInfo(int userId, string password, string email);

    Task<TokenResult> LoginAsync(string username, string password);

    Task<TokenResult> RefreshTokenAsync(string token, string refreshToken);

    Task<TokenResult> EmailConfirmAsync(string email, string token);

    Task LinkOsuAccount(int userId, string osuAccountId);

    Task LinkDiscordAccount(int userId, string discordAccountId);

    string GetOsuLinkUrl();

    string GetDiscordLinkUrl();

    public Task<string?> GetOsuAccount(int userId);

    public Task<string?> GetDiscordAccount(int userId);

    public Task<int?> GetUserIdByOsuAccount(string osuAccountId);
}