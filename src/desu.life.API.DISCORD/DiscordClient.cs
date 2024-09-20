using desu.life.API.DISCORD.Models;
using desu.life.API.DISCORD.Settings;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace desu.life.API.DISCORD
{
    public class DiscordClient(DiscordSettings discordSettings, ILogger<DiscordClient> logger)
    {
        public async Task<DiscordUser?> GetUserInfoOAuthAsync(string code)
        {
            var token = await GetAccessTokenAsync(code);

            var result = await GetUserInfoAsync(token);

            return result;
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
                    .WithBasicAuth(clientId, clientSecret)
                    .PostUrlEncodedAsync(new
                    {
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

        public async Task<DiscordUser> GetUserInfoAsync(string accessToken)
        {
            var apiEndPoint = discordSettings.EndPointBase;

            try
            {
                var response = await $"{apiEndPoint}/oauth2/@me"
                    .WithOAuthBearerToken(accessToken) // 使用 OAuth 令牌
                    .GetJsonAsync<DiscordUser>();

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
