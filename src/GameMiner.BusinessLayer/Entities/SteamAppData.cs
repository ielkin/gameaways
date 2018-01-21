using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GameMiner.Core;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    public class SteamAppData
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StoreApplicationType ApplicationType { get; set; }
		
        [JsonProperty("name")]
        public string Name { get; set; }
		
		[JsonProperty("steam_appid")]
        public long Id { get; set; }

        [JsonProperty("is_free")]
        public bool IsFree { get; set; }

        [JsonProperty("dlc")]
        public long[] Dlc { get; set; }
		
		[JsonProperty("header_image")]
        public string LargeTileUrl { get; set; }
		
		[JsonProperty("price_overview")]
        public PriceInformation PriceInformation { get; set; }

        [JsonProperty("fullgame")]
        public SteamApp FullGame { get; set; }

        [JsonProperty("detailed_description")]
        public string Description { get; set; }

        [JsonProperty("release_date")]
        public ReleaseDate ReleaseDate { get; set; }
    }
}