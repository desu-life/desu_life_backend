using desu.life.Requests;
using desu.life.Responses;
using desu.life.Services;
using desu.life.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace desu.life.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, OsuSettings osuSettings, DiscordSettings discordSettings) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly OsuSettings _osuSettings = osuSettings;
    private readonly DiscordSettings _discordSettings = discordSettings;

    [HttpPost("Register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _userService.RegisterAsync(request.UserName, request.Password, request.Email);
        if (!result.Success)
            return BadRequest(new FailedResponse
            {
                Errors = result.Errors!
            });
        return Ok(new TokenResponse
        {
            AccessToken = result.AccessToken,
            TokenType = result.TokenType,
            ExpiresIn = result.ExpiresIn,
            RefreshToken = result.RefreshToken
        });
    }

    [HttpPost("EmailConfirm")]
    public async Task<IActionResult> EmailConfirm(EmailConfirmRequest request)
    {
        var result = await _userService.EmailConfirmAsync(request.Email, request.Token);
        if (!result.Success)
            return BadRequest(new FailedResponse
            {
                Errors = result.Errors!
            });
        return Ok(new TokenResponse
        {
            AccessToken = result.AccessToken,
            TokenType = result.TokenType,
            ExpiresIn = result.ExpiresIn,
            RefreshToken = result.RefreshToken
        });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _userService.LoginAsync(request.Email, request.Password);
        if (!result.Success)
            return Unauthorized(new FailedResponse
            {
                Errors = result.Errors!
            });

        return Ok(new TokenResponse
        {
            AccessToken = result.AccessToken,
            TokenType = result.TokenType,
            ExpiresIn = result.ExpiresIn,
            RefreshToken = result.RefreshToken
        });
    }

    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _userService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
        if (!result.Success)
            return Unauthorized(new FailedResponse
            {
                Errors = result.Errors!
            });

        return Ok(new TokenResponse
        {
            AccessToken = result.AccessToken,
            TokenType = result.TokenType,
            ExpiresIn = result.ExpiresIn,
            RefreshToken = result.RefreshToken
        });
    }

    [HttpGet("LinkOsu")]
    [Authorize(Policy = "LinkAccount")]
    public IActionResult LinkOsu()
    {
        return Ok(new LinkResponse { RedirectUrl = _userService.GetOsuLinkUrl() });
    }

    [HttpGet("LinkDiscord")]
    [Authorize(Policy = "LinkAccount")]
    public IActionResult LinkDiscord()
    {
        return Ok(new LinkResponse { RedirectUrl = _userService.GetDiscordLinkUrl() });
    }
}