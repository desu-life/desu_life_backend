using desu.life.Requests;
using desu.life.Responses;
using desu.life.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace desu.life.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CallbackController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("LinkOsu")]
    [Authorize(Policy = "LinkAccount")]
    public async Task<IActionResult> LinkOsuAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        // TODO：从返回的数据中获取osuAccountId

        var osuAccountId = "1";
        await _userService.LinkOsuAccount(int.Parse(userId), osuAccountId);

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