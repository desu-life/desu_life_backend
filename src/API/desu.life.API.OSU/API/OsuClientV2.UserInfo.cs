using desu.life.API.OSU.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace desu.life.API;

public partial class OsuClientV2
{
    public async Task<OAuthUserProfile?> GetUserInfoOAuthAsync(string code)
    {
        var token = await GetOauthTokenAsync(code);
        if (token is null) return null;

        var result = await _osuEndpointV2.AppendPathSegment("me")
                                         .WithOAuthBearerToken(token)
                                         .AllowHttpStatus("200,404")
                                         .GetAsync();

        if (result.StatusCode == 404) return null;
        return await result.GetJsonAsync<OAuthUserProfile>();
    }

    public async Task<UserProfile?> GetUserInfoAsync(string username, string mode)
    {
        await CheckOsuPublicTokenAsync();
        if (_publicToken is null) return null;

        var result = await OsuHttp().AppendPathSegments(["users", username, mode])
                                    .SetQueryParam("key", "username")
                                    .GetAsync();

        if (result.StatusCode == 404) return null;
        return await result.GetJsonAsync<UserProfile>();
    }

    public async Task<UserProfile?> GetUserInfoAsync(long osu_uid, string mode)
    {
        await CheckOsuPublicTokenAsync();
        if (_publicToken is null) return null;

        var result = await OsuHttp().AppendPathSegments(["users", osu_uid, mode])
                                    .SetQueryParam("key", "id")
                                    .GetAsync();

        if (result.StatusCode == 404) return null;
        return await result.GetJsonAsync<UserProfile>();
    }


}