#nullable disable

namespace desu.life.Requests;

public class LoginRequest
{
    public string Email { get; set; }

    public string Password { get; set; }
}