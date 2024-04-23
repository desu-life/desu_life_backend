using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desu_life_backend.Controllers;

[ApiController]
[Authorize(Roles = "Admin")] // ��֤����Ա��ݵ�¼
[Route("[controller]")]
public class Demo2Controller : ControllerBase
{
    [HttpGet(Name = "GetWelcome")]
    public string Get()
    {
        return "Hello admin!";
    }
}