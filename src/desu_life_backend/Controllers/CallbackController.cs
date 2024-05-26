using desu.life.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace desu.life.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallbackController(IUserService userService, ILogger<CallbackController> logger, API.OSU osuApiService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger _logger = logger;
    private readonly API.OSU _osuApiService = osuApiService;

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