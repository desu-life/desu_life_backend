using desu.life.Settings;
using Flurl;
using Flurl.Http;
using System.Net;

namespace desu.life.API;

public partial class OSU
{
    private readonly string _osuEndpointV2 = osuSettings.EndPointBase + "/api/v2";
    private readonly string _osuTokenEndpoint = osuSettings.EndPointBase + "/oauth/token";



}