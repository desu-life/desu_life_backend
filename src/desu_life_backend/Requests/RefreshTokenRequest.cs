#nullable disable

using System.Text.Json.Serialization;

namespace desu_life_backend.Requests;

/// <summary>
/// RefreshToken 请求参数
/// </summary>
public class RefreshTokenRequest
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}