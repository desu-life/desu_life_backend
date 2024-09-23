using desu.life.API.DISCORD.Models;
using desu.life.API.DISCORD.Settings;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using static System.Formats.Asn1.AsnWriter;

namespace desu.life.API.DISCORD
{
    public class DiscordClient(DiscordSettings discordSettings, ILogger<DiscordClient> logger)
    {
        public async Task<DiscordUser?> GetUserInfoOAuthAsync(string code)
        {
            var token = await GetAccessTokenAsync(code);

            var result = await GetUserInfoAsync(token);

            return result.DiscordUser;
        }


        public async Task<string> GetAccessTokenAsync(string code)
        {
            var clientId = discordSettings.ClientID;
            var clientSecret = discordSettings.ClientSecret;
            var redirectUri = discordSettings.RedirectUri;
            var apiEndPoint = discordSettings.EndPointBase;


            try
            {
                var response = await $"{apiEndPoint}/oauth2/token"
                 
                    .PostUrlEncodedAsync(new
                    {
                        client_id = clientId,
                        client_secret = clientSecret,
                        grant_type = "authorization_code",
                        code = code,
                        redirect_uri = redirectUri
                    })

                    .ReceiveJson<DiscordToken>();

                return response.AccessToken;
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseStringAsync();
                throw new Exception($"Error fetching access token: {error}");
            }
        }

        public async Task<Root> GetUserInfoAsync(string accessToken)
        {
            var apiEndPoint = discordSettings.EndPointBase;

            try
            {
                var response = await $"{apiEndPoint}/oauth2/@me"
                    .BeforeCall(call => {

                        // 获取请求方法
                        var method = call.Request.Verb.ToString();

                        // 获取请求 URL
                        var url = call.Request.Url.ToString();

                        // 获取请求头
                        var headers = call.Request.Headers;
                        var headerString = string.Join("\r\n", headers.Select(h => $"{h.Name}: {string.Join(", ", h.Value)}"));

                        // 获取请求体
                        var content = call.Request.Content?.ReadAsStringAsync().Result ?? "";

                        // 打印完整的 HTTP 请求报文
                        Console.WriteLine($"{method} {url}");
                        Console.WriteLine(headerString);
                        Console.WriteLine("Content-Length: " + content.Length);
                        Console.WriteLine();
                        Console.WriteLine(content);
                    })
                    .WithOAuthBearerToken(accessToken) // 使用 OAuth 令牌
                    .GetJsonAsync<Root>();

                return response;
            }
            catch (FlurlHttpException ex)
            {
                // 错误处理，输出响应内容
                var error = await ex.GetResponseStringAsync();
                throw new Exception($"Error fetching user info: {error}");
            }
        }
    }
}
