using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desu_life_backend.Controllers;

[ApiController]
[Authorize] // ��֤��¼
[Route("[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet(Name = "GetWelcome")]
    public string Get()
    {
        return "Hello user!";
    }
}