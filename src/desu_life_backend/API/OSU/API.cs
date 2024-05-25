using desu.life.Settings;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace desu.life.API.OSU
{
    public class API(IHttpClientFactory httpClientFactory, OsuSettings osuSettings, ILogger<API> logger)
    {
        private readonly OsuSettings _osuSettings = osuSettings;
        private readonly ILogger _logger = logger;
        private readonly HttpClient _apiHttpClient = httpClientFactory.CreateClient("OsuAPIBase");
        private readonly string _osuApiEndPointV2 = "/api/v2";

        public async Task<long> GetUserInfoByOAuthCodeAsync(string _code)
        {
            try
            {
                var requestData = new
                {
                    grant_type = "authorization_code",
                    client_id = _osuSettings.ClientID,
                    client_secret = _osuSettings.ClientSecret,
                    code = _code,
                    redirect_uri = _osuSettings.RedirectUri,
                };
                var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
                var response = await _apiHttpClient.PostAsync("/oauth/token", jsonContent);

                response.EnsureSuccessStatusCode();
                var contentType = response.Content.Headers.ContentType?.MediaType;

                var responseString = await response.Content.ReadAsStringAsync();

                // if (contentType == "application/json") { }
                if (contentType == "text/html")
                {
                    // code 过期
                    return -1;
                }

                using var responseBody = JsonDocument.Parse(responseString);
                // Get access token
                string access_token = responseBody.RootElement.GetProperty("access_token").GetString()!;
                try
                {
                    var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_osuApiEndPointV2}/me");
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                    var userInfoResponse = await _apiHttpClient.SendAsync(requestMessage);
                    userInfoResponse.EnsureSuccessStatusCode();
                    var userInfoResponseString = await userInfoResponse.Content.ReadAsStringAsync();

                    // return userInfoResponseString;

                    using var userInfoResponseBody = JsonDocument.Parse(userInfoResponseString);
                    // Get osu user id from response data
                    if (!userInfoResponseBody.RootElement.TryGetProperty("id", out JsonElement idElement))
                    {
                        _logger.LogWarning("{CurrentTime} Osu!API 失败，无法从应答中获取osu id。", $"[{DateTime.UtcNow}]");
                        return -1;
                    }
                    var osu_uid = idElement.GetInt64();
                    return osu_uid;
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogWarning("{CurrentTime} Osu!API 失败。错误信息：{Message}", $"[{DateTime.UtcNow}]", ex.Message);
                    return -1;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning("{CurrentTime} Osu!API 失败。错误信息：{Message}", $"[{DateTime.UtcNow}]", ex.Message);
                return -1;
            }

        }
    }
}