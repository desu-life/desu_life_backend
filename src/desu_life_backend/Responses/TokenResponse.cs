using System.Text.Json.Serialization;

namespace desu_life_backend.Responses
{
    /// <summary>
    /// 注册、登录成功后返回 token
    /// </summary>
    public class TokenResponse
    {
        [JsonPropertyName("access_token")] 
        public string AccessToken { get; set; }
    
        [JsonPropertyName("token_type")] 
        public string TokenType { get; set; }
    }
}
