using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    public class AppList
    {
        [JsonProperty("apps")]
        public IList<SteamApp> Apps { get; set; }
    }
}