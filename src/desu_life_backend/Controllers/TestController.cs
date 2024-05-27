using desu.life.Utils.RedeemCode;
using Microsoft.AspNetCore.Mvc;

namespace desu.life.Controllers;

// roles = [ "System", "Bot", "Administrator", "Moderator", "CoOrganizer", "PremiumUser", "User" ];

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("cdkeytest")]
    public string Get()
    {
        var cdkey = RedeemCodeGenerator.Generate();
        return $"Hello! Your CDKey is {cdkey}";
    }
}