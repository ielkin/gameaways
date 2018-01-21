using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    public class PlayerSummary
    {
        [JsonProperty("steamid")]
        public long Id { get; set; }

        [JsonProperty("communityvisibilitystate")]
        public CommunityVisibilityState VisibilityState { get; set; }

        [JsonProperty("profilestate")]
        public int ProfileState { get; set; }

        [JsonProperty("personaname")]
        public string SteamName { get; set; }

        [JsonProperty("realname")]
        public string FullName { get; set; }

        [JsonProperty("lastlogoff")]
        public long LastLogoffTimestamp { get; set; }

        [JsonProperty("profileurl")]
        public string ProfileUrl { get; set; }

        [JsonProperty("avatar")]
        public string AvatarSmallUrl { get; set; }

        [JsonProperty("avatarmedium")]
        public string AvatarMediumUrl { get; set; }

        [JsonProperty("avatarfull")]
        public string AvatarFullUrl { get; set; }

        [JsonProperty("personastate")]
        public int SteamStatus { get; set; }

        [JsonProperty("primaryclanid")]
        public long PrimaryGroupId { get; set; }

        [JsonProperty("timecreated")]
        public long SteamMemberSinceTimestamp { get; set; }

        [JsonProperty("personastateflags")]
        public int SteamStatusFlags { get; set; }
    }
}