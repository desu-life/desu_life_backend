using System.Security.Claims;
using desu.life.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desu.life.Controllers;
/// <summary>
/// 回调相关接口，用于使用回调信息绑定账号
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CallbackController(IUserService userService, ILogger<CallbackController> logger, API.OsuClientV2 osuApiService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger _logger = logger;
    private readonly API.OsuClientV2 _osuApiService = osuApiService;

    /// <summary>
    /// osu! OAuth2回调跳转后 确认绑定用户接口
    /// </summary>
    /// <param name="code">osu!跳转用户面板时URL携带的code</param>
    /// <returns>空返回体</returns>
    [HttpGet("LinkOsu")]
    // [Authorize(Policy = "LinkAccount")]
    public async Task<IActionResult> LinkOsuAsync([FromQuery] string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return BadRequest();
        }

        // 这里需要前端做一下token传递
        var userId = "1";
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // if (userId == null)
        // {
        //     return Unauthorized();
        // }

        // 获取 osuAccountId
        var osuAccountInfo = await _osuApiService.GetUserInfoOAuthAsync(code);
        if (osuAccountInfo is null) return BadRequest();

        // _logger.LogInformation("Osu UID is {}.", osuAccountInfo.Id);

        await _userService.LinkOsuAccount(int.Parse(userId), osuAccountInfo.Id.ToString());

        return Ok();
    }
    /// <summary>
    /// Discord OAuth2回调跳转后 确认绑定用户接口
    /// </summary>
    /// <param name="code">Discord跳转用户面板时URL携带的code</param>
    /// <returns>空返回体</returns>
    [HttpGet("LinkDiscord")]
    [Authorize(Policy = "LinkAccount")]
    public async Task<IActionResult> LinkDiscordAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        // TODO：从返回的数据中获取DiscordAccountId

        var discordAccountId = "1";
        await _userService.LinkDiscordAccount(int.Parse(userId), discordAccountId);

        return Ok();
    }

}