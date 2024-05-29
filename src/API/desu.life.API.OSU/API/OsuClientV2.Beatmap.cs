using desu.life.API.OSU.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using static System.Net.WebRequestMethods;

namespace desu.life.API;

public partial class OsuClientV2
{
    public async Task<Beatmap?> GetBeatmap(long beatmapId)
    {
        await CheckTokenAsync();
        if (token.IsExpired) return null;

        var result = await OsuHttp().AppendPathSegments(["beatmaps", beatmapId])
                                    .GetAsync();

        if (result.StatusCode == 404) return null;
        return await result.GetJsonAsync<Beatmap>();
    }
}