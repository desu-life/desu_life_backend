using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desu_life_backend.Controllers;

[ApiController]
[Authorize(Roles = "Admin")] // 验证管理员身份登录
[Route("[controller]")]
public class Demo2Controller : ControllerBase
{
    [HttpGet(Name = "GetWelcome")]
    public string Get()
    {
        return "Hello admin!";
    }
}