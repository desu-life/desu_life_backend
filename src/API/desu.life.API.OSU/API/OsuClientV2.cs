using desu.life.Settings;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace desu.life.API;

public partial class OsuClientV2(OsuSettings osuSettings, ILogger<OsuClientV2> logger)
{
    private readonly ILogger _logger = logger;

    IFlurlRequest OsuHttp()
    {
        return _osuEndpointV2
        .WithOAuthBearerToken(_publicToken)
        .AllowHttpStatus("200,404");
    }
}