using desu.life.API.OSU.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace desu.life.API;

public partial class OsuClientV2
{
    private async Task<List<Score>?> GetUserScoresAsync(long osu_uid, string _mode, string type, int _legacy_only = 0,
                                               int _include_fails = 0, int _limit = 100, int _offset = 0)
    {
        await CheckTokenAsync();
        if (token.IsExpired) return null;

        var result = await OsuHttp().AppendPathSegments(["users", osu_uid, "scores", type])
                                    .SetQueryParams(new
                                    {
                                        mode = _mode,
                                        legacy_only = _legacy_only,
                                        include_fails = _include_fails,
                                        limit = _limit,
                                        offset = _offset,
                                    })
                                    .GetAsync();

        if (result.StatusCode == 404) return null;
        return await result.GetJsonAsync<List<Score>>();
    }

    public async Task<List<Score>?> GetUserBeatmapScoresAsync(long osu_uid, long beatmap_id, string _mode, int _legacy_only)
    {
        await CheckTokenAsync();
        if (token.IsExpired) return null;

        var request = OsuHttp().SetQueryParams(
            new
            {
                mode = _mode,
                legacy_only = _legacy_only,
            });

        request.AppendPathSegments(["beatmaps", beatmap_id, "scores", "users", osu_uid, "all"]);


        var result = await request.GetAsync();
        if (result.StatusCode == 404) return null;


        var scores = await result.GetJsonAsync<BeatmapScores>();
        return scores.Scores;

    }

    public async Task<BeatmapUserScore?> GetUserBeatmapScoreAsync(long osu_uid, long beatmap_id, string _mode, int _legacy_only)
    {
        await CheckTokenAsync();
        if (token.IsExpired) return null;

        var request = OsuHttp().SetQueryParams(
            new
            {
                mode = _mode,
                legacy_only = _legacy_only,
            });

        request.AppendPathSegments(["beatmaps", beatmap_id, "scores", "users", osu_uid]);

        var result = await request.GetAsync();
        if (result.StatusCode == 404) return null;

        var score = await result.GetJsonAsync<BeatmapUserScore>();
        return score;
    }

    public async Task<List<Score>?> GetUserBestScoresAsync(long osu_uid, string mode, int limit, int offset = 0, int legacy_only = 0)
    {
        return await GetUserScoresAsync(osu_uid, mode, "best", legacy_only, 0, limit, offset);
    }

    public async Task<List<Score>?> GetUserFirstScoresAsync(long osu_uid, string mode, int limit, int offset = 0, int legacy_only = 0)
    {
        return await GetUserScoresAsync(osu_uid, mode, "firsts", legacy_only, 0, limit, offset);
    }

    public async Task<List<Score>?> GetUserRecentScoresAsync(long osu_uid, string mode, int include_fails, int limit, int offset = 0, int legacy_only = 0)
    {
        return await GetUserScoresAsync(osu_uid, mode, "recent", legacy_only, include_fails, limit, offset);
    }

}