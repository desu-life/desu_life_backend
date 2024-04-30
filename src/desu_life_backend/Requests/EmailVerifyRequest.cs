#nullable disable

namespace desu.life.Requests;

public class EmailVerifyRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
}