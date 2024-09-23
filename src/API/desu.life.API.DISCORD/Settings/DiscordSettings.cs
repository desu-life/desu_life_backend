using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desu.life.API.DISCORD.Settings
{
    public class DiscordSettings
    {
        public required string ClientID { get; init; }
        public required string ClientSecret { get; init; }
        public required string RedirectUri { get; init; }
        public required string EndPointBase { get; init; }
    }
}
