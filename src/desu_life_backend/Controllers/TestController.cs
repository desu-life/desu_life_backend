using desu.life.API;
using desu.life.Services.User;
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


}