using desu.life.Settings;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace desu.life.API;

/// <summary>
/// 采用client_credentials方式的osu!api v2客户端
/// </summary>
/// <param name="osuSettings"></param>
/// <param name="logger"></param>
public partial class OsuClientV2(OsuSettings osuSettings, ILogger<OsuClientV2> logger)
{
    private readonly ILogger _logger = logger;

    IFlurlRequest OsuHttp()
    {
        return _osuEndpointV2
        .WithOAuthBearerToken(token.PublicToken)
        .AllowHttpStatus("200,404");
    }
}