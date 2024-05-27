using Flurl;
using Flurl.Http;
using static desu.life.API.OSU.Models;

namespace desu.life.API;

public partial class OSU
{
    public async Task<UserProfile?> GetUserInfoOAuthAsync(string code)
    {
        var token = await GetOauthTokenAsync(code);
        if (token is null) return null;

        return await _osuEndpointV2.AppendPathSegment("/me")
                                         .WithOAuthBearerToken(token)
                                         .GetJsonAsync<UserProfile>();
    }
}