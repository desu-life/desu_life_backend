using desu.life.API;
using desu.life.Services;
using desu.life.Utils.RedeemCode;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace desu.life.Controllers;

// roles = [ "System", "Bot", "Administrator", "Moderator", "CoOrganizer", "PremiumUser", "User" ];

[ApiController]
[Route("[controller]")]
public class TestController(IUserService userService, OsuClientV2 osuClientV2) : ControllerBase
{
    // 测试用途

    private readonly IUserService _userService = userService;

    private readonly OsuClientV2 _osuClientV2 = osuClientV2;

    [HttpGet("cdkeytest")]
    public string Get()
    {
        var cdkey = RedeemCodeGenerator.Generate();
        return $"Hello! Your CDKey is {cdkey}";
    }

    [HttpGet("get_user")]
    public async Task<string> GetUserAsync(string username)
    {
        // 从osu!api获取用户信息
        var user = await _osuClientV2.GetUserInfoAsync(username, "osu");
        if (user is null) return "null";

        // 从数据库中获取osu用户id
        bool linked = await _userService.GetUserIdByOsuAccount(user.Id.ToString()) != null;

        return user.ToJson();
    }

    [HttpGet("get_beatmap")]
    public async Task<string> GetBeatmapAsync(long beatmap_id)
    {
        // 从osu!api获取谱面信息
        var beatmap = await _osuClientV2.GetBeatmap(beatmap_id);
        if (beatmap is null) return "null";

        return beatmap.ToJson();
    }

    [HttpGet("get_user_best_scores")]
    public async Task<string> GetUserScoresAsync(long osu_uid, string mode)
    {
        // 从osu!api获取用户最佳成绩
        var scores = await _osuClientV2.GetUserBestScoresAsync(osu_uid, mode, 100);
        if (scores is null) return "null";

        return scores.ToJson();
    }

    [HttpGet("get_user_beatmap_scores")]
    public async Task<string> GetUserBeatmapScoresAsync(long userId, long beatmap_id, string mode)
    {
        // 从osu!api获取用户在谱面上的成绩
        var scores = await _osuClientV2.GetUserBeatmapScoresAsync(userId, beatmap_id, mode, 0);
        if (scores is null) return "null";

        return scores.ToJson();
    }

    [HttpGet("get_user_beatmap_score")]
    public async Task<string> GetUserBeatmapScoreAsync(long userId, long beatmap_id, string mode)
    {
        // 从osu!api获取用户在谱面上的成绩
        var scores = await _osuClientV2.GetUserBeatmapScoreAsync(userId, beatmap_id, mode, 0);
        if (scores is null) return "null";

        return scores.ToJson();
    }
}