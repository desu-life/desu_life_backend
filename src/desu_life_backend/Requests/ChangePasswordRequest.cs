using Microsoft.Build.Framework;

namespace desu.life.Requests;
/// <summary>
/// ×¢²áºó²¹ÌîĞÅÏ¢ÇëÇó
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// µÇÂ¼ÃÜÂë
    /// </summary>
    [Required] public string Password { get; set; }

    /// <summary>
    /// ÓÊÏä
    /// </summary>
    [Required] public string Email { get; set; }
}