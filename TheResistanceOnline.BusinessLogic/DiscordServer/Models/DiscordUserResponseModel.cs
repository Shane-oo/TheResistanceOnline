using Newtonsoft.Json;

namespace TheResistanceOnline.BusinessLogic.DiscordServer.Models
{
    public class DiscordUserResponseModel
    {
        #region Properties

        [JsonProperty("avatar")]
        public string AvatarHash { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        #endregion
    }
}
