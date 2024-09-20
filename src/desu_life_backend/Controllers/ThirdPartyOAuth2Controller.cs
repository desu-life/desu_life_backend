using System.Security.Claims;
using desu.life.API.DISCORD;
using desu.life.Data.Models;
using desu.life.Responses;
using desu.life.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace desu.life.Controllers;
/// <summary>
/// 第三方平台OAuth2相关接口，用于生成第三方平台跳转链接、根据回调code获取第三方平台用户信息
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ThirdPartyOAuth2Controller(
    IUserService userService,
    ILogger<ThirdPartyOAuth2Controller> logger,
    API.OsuClientV2 osuApiService, 
    DiscordClient discordClient) : ControllerBase
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// osu! OAuth2触发授权跳转接口
    /// </summary>
    /// <returns></returns>
    [HttpGet("RedirectOsuLogin")]
    public string RedirectOsuLogin()
    {
        return _userService.GetOsuLinkUrl();
    }

    /// <summary>
    /// Discord OAuth2触发授权跳转接口
    /// </summary>
    /// <returns></returns>
    [HttpGet("RedirectDiscordOAuth")]
    [Authorize(Policy = "LinkAccount")]
    public string RedirectDiscordOAuth()
    {
        return _userService.GetDiscordLinkUrl();
    }



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
        var osuAccountInfo = await osuApiService.GetUserInfoOAuthAsync(code);
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
    public async Task<IActionResult> LinkDiscordAsync([FromQuery] string? code)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var discordUserInfo = await discordClient.GetUserInfoOAuthAsync(code);

        var discordAccountId = discordUserInfo.Id;
        await _userService.LinkDiscordAccount(int.Parse(userId), discordAccountId);

        return Ok();
    }

}