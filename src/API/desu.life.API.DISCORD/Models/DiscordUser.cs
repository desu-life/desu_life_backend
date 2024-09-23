using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace desu.life.API.DISCORD.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class ApplicationModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("bot")]
        public Bot Bot { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("is_monetized")]
        public bool IsMonetized { get; set; }

        [JsonPropertyName("is_verified")]
        public bool IsVerified { get; set; }

        [JsonPropertyName("bot_public")]
        public bool BotPublic { get; set; }

        [JsonPropertyName("bot_require_code_grant")]
        public bool BotRequireCodeGrant { get; set; }

        [JsonPropertyName("integration_types_config")]
        public IntegrationTypesConfig IntegrationTypesConfig { get; set; }

        [JsonPropertyName("verify_key")]
        public string VerifyKey { get; set; }

        [JsonPropertyName("flags")]
        public int Flags { get; set; }

        [JsonPropertyName("hook")]
        public bool Hook { get; set; }

        [JsonPropertyName("storefront_available")]
        public bool StorefrontAvailable { get; set; }
    }

    public class Bot
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; }

        [JsonPropertyName("public_flags")]
        public int PublicFlags { get; set; }

        [JsonPropertyName("flags")]
        public int Flags { get; set; }

        [JsonPropertyName("bot")]
        public bool IsBot { get; set; }

        [JsonPropertyName("banner")]
        public string Banner { get; set; }

        [JsonPropertyName("accent_color")]
        public string AccentColor { get; set; }

        [JsonPropertyName("global_name")]
        public string GlobalName { get; set; }

        [JsonPropertyName("avatar_decoration_data")]
        public string AvatarDecorationData { get; set; }

        [JsonPropertyName("banner_color")]
        public string BannerColor { get; set; }

        [JsonPropertyName("clan")]
        public string Clan { get; set; }
    }

    public class IntegrationTypesConfig
    {
        [JsonPropertyName("0")]
        public IntegrationType Zero { get; set; }
    }

    public class IntegrationType
    {
        // Add fields as necessary based on the actual structure of "IntegrationType"
    }

    public class DiscordUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; }

        [JsonPropertyName("public_flags")]
        public int PublicFlags { get; set; }

        [JsonPropertyName("flags")]
        public int Flags { get; set; }

        [JsonPropertyName("banner")]
        public string Banner { get; set; }

        [JsonPropertyName("accent_color")]
        public string AccentColor { get; set; }

        [JsonPropertyName("global_name")]
        public string GlobalName { get; set; }

        [JsonPropertyName("avatar_decoration_data")]
        public string AvatarDecorationData { get; set; }

        [JsonPropertyName("banner_color")]
        public string BannerColor { get; set; }

        [JsonPropertyName("clan")]
        public string Clan { get; set; }
    }

    public class Root
    {
        [JsonPropertyName("application")]
        public ApplicationModel Application { get; set; }

        [JsonPropertyName("expires")]
        public DateTime Expires { get; set; }

        [JsonPropertyName("scopes")]
        public string[] Scopes { get; set; }

        [JsonPropertyName("user")]
        public DiscordUser DiscordUser { get; set; }
    }


}
