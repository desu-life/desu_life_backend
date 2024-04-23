#nullable disable

namespace desu.life.Requests;

public class LoginRequest
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
}