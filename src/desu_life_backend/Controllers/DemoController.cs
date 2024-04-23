﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace desu_life_backend.Controllers;

[ApiController]
[Authorize] // 验证登录
[Route("[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet(Name = "GetWelcome")]
    public string Get()
    {
        return "Hello user!";
    }
}