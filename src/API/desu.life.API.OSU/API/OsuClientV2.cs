using desu.life.Settings;
using Flurl.Http;
using Microsoft.Extensions.Logging;

namespace desu.life.API;

public partial class OsuClientV2(OsuSettings osuSettings, ILogger<OsuClientV2> logger)
{
    private readonly ILogger _logger = logger;


    private string _publicToken = "";
    private string _lazerToken = "";

    private long _publicTokenExpireTime = 0;
    private long _lazerTokenExpireTime = 0;

    IFlurlRequest Http()
    {
        // CheckToken().Wait();

        return _osuEndpointV2
        .WithOAuthBearerToken(_publicToken)
        .AllowHttpStatus("200,404");
    }

}