using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desu.life.Controllers;


[ApiController]
[Route("[controller]")]
public class GuestController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello guest!";
    }
}

[ApiController]
[Authorize(Roles = "User")] // 验证登录
[Route("[controller]")]
public class NormalUserController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello user!";
    }
}

[ApiController]
[Authorize(Roles = "Administrator")] // 验证管理员身份登录
[Route("[controller]")]
public class AdminController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "Hello admin!";
    }
}