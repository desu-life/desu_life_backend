#nullable disable

using System.ComponentModel.DataAnnotations;

namespace desu.life.Requests;

public class RegisterRequest
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }

    [Required] public string Email { get; set; }
}