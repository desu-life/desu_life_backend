#nullable disable

using System.Text.Json.Serialization;

namespace desu.life.Responses;

/// <summary>
/// 注册、登录成功后返回 token
/// </summary>
public class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}