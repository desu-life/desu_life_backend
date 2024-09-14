using System.Security.Claims;
using desu.life.Data.Models;
using desu.life.Responses;
using desu.life.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    /// osu! OAuth2回调跳转后 自动登录/注册并绑定接口
    /// </summary>
    /// <param name="code">osu!跳转用户面板时URL携带的code</param>
    /// <returns>空返回体</returns>
    [HttpGet("LinkOsu")]
    public async Task<IActionResult> LinkOsuAsync([FromQuery] string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return BadRequest();
        }

        // 获取 osu!账号信息
        var osuAccountInfo = await _osuApiService.GetUserInfoOAuthAsync(code);
        if (osuAccountInfo is null) return BadRequest();

        var result = await userService.RegisterOrLogin(osuAccountInfo.Username, osuAccountInfo.Id.ToString());

        return Ok(new TokenResponse
        {
            AccessToken = result.AccessToken,
            TokenType = result.TokenType,
            ExpiresIn = result.ExpiresIn,
            RefreshToken = result.RefreshToken
        });
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