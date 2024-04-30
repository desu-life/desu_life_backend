#nullable disable

namespace desu.life.Requests;

public class RegisterRequest
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string Email { get; set; }
}