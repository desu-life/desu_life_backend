#nullable disable

namespace desu.life.Requests;

public class EmailConfirmRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
}