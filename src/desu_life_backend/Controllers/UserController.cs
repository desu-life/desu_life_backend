using desu.life.Requests;
using desu.life.Responses;
using desu.life.Services.User;
using desu.life.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using desu.life.API.DISCORD.Settings;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using desu.life.Extensions;
using Microsoft.Extensions.Options;

namespace desu.life.Controllers;
/// <summary>
/// 用户相关接口
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, OsuSettings osuSettings, DiscordSettings discordSettings, IAuthorizationPolicyProvider policyProvider, IOptions<AuthorizationOptions> options) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly OsuSettings _osuSettings = osuSettings;
    private readonly DiscordSettings _discordSettings = discordSettings;
    private readonly AuthorizationOptions _authorizationOptions = options.Value;
    /// <summary>
    /// 用户补填邮箱、密码接口
    /// </summary>
    /// <param name="request">补填请求</param>
    /// <returns>空返回体</returns>
    [HttpPost("FillLoginInfo")]
    public async Task<IActionResult> FillLoginInfo(FillLoginInfoRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        await _userService.FillLoginInfo(Convert.ToInt32(userId), request.Password, request.Email);

        return Ok();
    }
    /// <summary>
    /// 邮箱验证接口
    /// </summary>
    /// <param name="request">验证请求</param>
    /// <returns>空返回体</returns>
    [HttpPost("EmailConfirm")]
    public async Task<IActionResult> EmailConfirm(EmailConfirmRequest request)
    {
        await _userService.EmailConfirmAsync(request.Email, request.Token);

        return Ok();
    }

    /// <summary>
    /// 邮箱、密码登录接口
    /// </summary>
    /// <param name="request">登录请求</param>
    /// <returns>Token信息</returns>
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

    /// <summary>
    /// 刷新Token接口
    /// </summary>
    /// <param name="request">刷新请求</param>
    /// <returns>Token信息</returns>
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

    /// <summary>
    /// 获取当前用户的role以及可用的policy
    /// </summary>
    /// <returns></returns>
    [HttpGet("Me")]
    public async Task<MyInfoResponse> Me()
    {
        var response = new MyInfoResponse();

        var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        var allowedPolicies = new List<string>();

        // 遍历所有注册的策略
        foreach (var policy in _authorizationOptions.GetPolicies()) // GetPolicies 是你需要实现的获取策略列表的方法
        {
            var canAccess = policy.Value.Requirements
                .OfType<RolesAuthorizationRequirement>()
                .Any(requirement => userRoles.Any(role => requirement.AllowedRoles.Contains(role)));

            if (canAccess)
            {
                allowedPolicies.Add(policy.Key);
            }
        }

        response.allowedPolicies = allowedPolicies;
        response.roles = userRoles;
        response.name = User.Identity?.Name;

        return response;


    }
}