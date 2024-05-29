using desu.life.API.OSU.Models;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace desu.life.API;

public partial class OsuClientV2
{
    private async Task<List<BeatmapScore>?> GetUserScoresAsync(long osu_uid, string _mode, string type, int _legacy_only = 0,
                                               int _include_fails = 0, int _limit = 100, int _offset = 0)
    {
        await CheckOsuPublicTokenAsync();
        if (_publicToken is null) return null;

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
        return await result.GetJsonAsync<List<BeatmapScore>>();
    }

    public async Task<List<BeatmapScore>?> GetUserBeatmapScoresAsync(long osu_uid, long beatmap_id, string _mode, int _legacy_only, bool get_all)
    {
        await CheckOsuPublicTokenAsync();
        if (_publicToken is null) return null;

        var request = OsuHttp().SetQueryParams(
            new
            {
                mode = _mode,
                legacy_only = _legacy_only,
            });

        List<BeatmapScore> beatmapScore = [];
        if (get_all) request.AppendPathSegments(["beatmaps", beatmap_id, "scores", "users", osu_uid, "all"]);
        else request.AppendPathSegments(["beatmaps", beatmap_id, "scores", "users", osu_uid]);

        var result = await request.GetAsync();
        if (result.StatusCode == 404) return null;

        if (get_all)
        {
            var scores = await result.GetJsonAsync<BeatmapScoreExtensions>();
            if (scores.Scores is null) return null;
            foreach (var score in scores.Scores) beatmapScore.Add(score);
        }
        else
        {
            var score = await result.GetJsonAsync<BeatmapScoreExtensions>();
            if (score.Score is null) return null;
            score.Score.Position = score.Position;
            beatmapScore.Add(score.Score);
        }

        return beatmapScore;
    }

    public async Task<List<BeatmapScore>?> GetUserBestScoresAsync(long osu_uid, string mode, int limit, int offset = 0, int legacy_only = 0)
    {
        return await GetUserScoresAsync(osu_uid, mode, "best", legacy_only, 0, limit, offset);
    }

    public async Task<List<BeatmapScore>?> GetUserFirstScoresAsync(long osu_uid, string mode, int limit, int offset = 0, int legacy_only = 0)
    {
        return await GetUserScoresAsync(osu_uid, mode, "firsts", legacy_only, 0, limit, offset);
    }

    public async Task<List<BeatmapScore>?> GetUserRecentScoresAsync(long osu_uid, string mode, int include_fails, int limit, int offset = 0, int legacy_only = 0)
    {
        return await GetUserScoresAsync(osu_uid, mode, "recent", legacy_only, include_fails, limit, offset);
    }

}