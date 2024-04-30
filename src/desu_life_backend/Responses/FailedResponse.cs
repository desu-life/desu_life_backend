namespace desu.life.Responses;

/// <summary>
/// 登录、注册失败时返回错误信息
/// </summary>
public class FailedResponse
{
    public IEnumerable<string> Errors { get; set; }
}