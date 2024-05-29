using System.Text.Json;
using desu.life.API.OSU.Models;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace desu.life.API;

public partial class OsuClientV2
{
    public Token token = new();

    public class Token
    {
        public string? PublicToken { get; private set; } = null;
        public string? RefreshToken { get; private set; } = null;
        public long? PublicTokenExpireTime { get; private set; } = null;

        private long AdjustTokenExpire(long expiresIn)
        {
            return (long)(expiresIn - (expiresIn * 0.05));
        }

        public void Update(TokenResponse resp)
        {
            PublicToken = resp.AccessToken;
            RefreshToken = resp.RefreshToken;
            PublicTokenExpireTime =
                AdjustTokenExpire(resp.ExpiresIn) + DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public bool IsExpired =>
            PublicTokenExpireTime is null
                ? true
                : PublicTokenExpireTime <= DateTimeOffset.Now.ToUnixTimeSeconds();
    }

    private async Task<TokenResponse> UpdateTokenAsync()
    {
        var requestData = new
        {
            grant_type = "client_credentials",
            client_id = osuSettings.ClientID,
            client_secret = osuSettings.ClientSecret,
            scope = "public",
            code = "desu.life",
        };

        var result = await _osuTokenEndpoint.PostJsonAsync(requestData);
        return await result.GetJsonAsync<TokenResponse>();
    }

    private async Task<TokenResponse> GetOauthTokenAsync(string _code)
    {
        var requestData = new
        {
            grant_type = "authorization_code",
            client_id = osuSettings.ClientID,
            client_secret = osuSettings.ClientSecret,
            code = _code,
            redirect_uri = osuSettings.RedirectUri,
        };

        var result = await _osuTokenEndpoint.PostJsonAsync(requestData);
        return await result.GetJsonAsync<TokenResponse>();
    }

    /// <summary>
    /// 检查token过期，然后重新获取
    /// </summary>
    /// <returns>返回值是实际token可用状态</returns>
    public async Task CheckTokenAsync()
    {
        if (token.IsExpired)
        {
            _logger.LogInformation("正在获取OSUApiV2_Token");
            try
            {
                var resp = await UpdateTokenAsync();
                token.Update(resp);
                _logger.LogInformation(
                    $"Token过期时间: {DateTimeOffset.FromUnixTimeSeconds(token.PublicTokenExpireTime!.Value).DateTime.ToLocalTime()}"
                );
                // _logger.LogInformation(string.Concat("获取成功, Token: ", _publicToken.AsSpan(Utils.TryGetConsoleWidth() - 38), "..."));
            }
            catch (Exception ex)
            {
                logger.LogWarning("获取token失败, Error: \n({})", ex);
            }
        }
    }
}
