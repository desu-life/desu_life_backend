using desu.life.Settings;
using Flurl;
using Flurl.Http;
using System.Net;

namespace desu.life.API;

public partial class OSU(OsuSettings osuSettings, ILogger<OSU> logger)
{
    private readonly OsuSettings _osuSettings = osuSettings;
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