#nullable disable

using System.ComponentModel.DataAnnotations;

namespace desu.life.Requests;
/// <summary>
/// 注册后补填信息请求
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// 登录密码
    /// </summary>
    [Required] public string Password { get; set; }
    
    /// <summary>
    /// 邮箱
    /// </summary>
    [Required] public string Email { get; set; }
}