using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    [JsonObject]
    public class SteamAppDataResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
		
		[JsonProperty("data")]
        public SteamAppData AppData { get; set; }
    }
}