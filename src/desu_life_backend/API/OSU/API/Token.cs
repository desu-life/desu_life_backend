using System.Text.Json;
using Flurl.Http;

namespace desu.life.API;

public partial class OSU
{
    private async Task<bool> GetPublicTokenAsync()
    {
        var requestData = new
        {
            grant_type = "client_credentials",
            client_id = _osuSettings.ClientID,
            client_secret = _osuSettings.ClientSecret,
            scope = "public",
            code = "desu.life",
        };

        var result = await _osuTokenEndpoint.PostJsonAsync(requestData);


        var body = await result.GetJsonAsync<JsonElement>();
        try
        {
            var token = body.GetProperty("access_token").GetString();
            if (token is null) return false;
            _publicToken = token;
            _publicTokenExpireTime = DateTimeOffset.Now.ToUnixTimeSeconds()
                                    + long.Parse((body.GetProperty("expires_in").ToString()) ?? "0");
            return true;
        }
        catch
        {
            _logger.LogInformation("获取token失败, 返回Body: \n({})", body.ToString());
            return false;
        }
    }

    public async Task CheckPublicTokenAsync()
    {
        if (_publicTokenExpireTime == 0)
        {
            _logger.LogInformation("正在获取OSUApiV2_Token");
            if (await GetPublicTokenAsync())
            {
                // _logger.LogInformation(string.Concat("获取成功, Token: ", _publicToken.AsSpan(Utils.TryGetConsoleWidth() - 38), "..."));
                _logger.LogInformation($"Token过期时间: {DateTimeOffset.FromUnixTimeSeconds(_publicTokenExpireTime).DateTime.ToLocalTime()}");
            }
        }
        else if (_publicTokenExpireTime <= DateTimeOffset.Now.ToUnixTimeSeconds())
        {
            _logger.LogInformation("OSUApiV2_Token已过期, 正在重新获取");
            if (await GetPublicTokenAsync())
            {
                // _logger.LogInformation(string.Concat("获取成功, Token: ", _publicToken.AsSpan(Utils.TryGetConsoleWidth() - 38), "..."));
                _logger.LogInformation($"Token过期时间: {DateTimeOffset.FromUnixTimeSeconds(_publicTokenExpireTime).DateTime.ToLocalTime()}");
            }
        }
    }

    private async Task<string?> GetOauthTokenAsync(string _code)
    {
        var requestData = new
        {
            grant_type = "authorization_code",
            client_id = _osuSettings.ClientID,
            client_secret = _osuSettings.ClientSecret,
            code = _code,
            redirect_uri = _osuSettings.RedirectUri,
        };


        var result = await _osuTokenEndpoint.PostJsonAsync(requestData);

        var body = await result.GetJsonAsync<JsonElement>();
        try
        {
            // 暂时没有结构体 先这样写
            return body.GetProperty("access_token").GetString() ?? null;
        }
        catch
        {
            _logger.LogInformation("获取token失败, 返回Body: \n({})", body.ToString());
            return null;
        }
    }

}