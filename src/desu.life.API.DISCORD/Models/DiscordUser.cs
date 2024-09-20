using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desu.life.API.DISCORD.Models
{
    public class DiscordUser
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
        public string Locale { get; set; }
        public bool MfaEnabled { get; set; }
    }
}
