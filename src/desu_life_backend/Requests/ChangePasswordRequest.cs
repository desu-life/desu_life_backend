using Microsoft.Build.Framework;

namespace desu.life.Requests;
/// <summary>
/// ע�������Ϣ����
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// ��¼����
    /// </summary>
    [Required] public string Password { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    [Required] public string Email { get; set; }
}