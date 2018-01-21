using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    public class SteamApp
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("appid")]
        public long? Id { get; set; }
    }
}
