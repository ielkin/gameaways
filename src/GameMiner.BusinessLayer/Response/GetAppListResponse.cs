using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    [JsonObject]
    public class GetAppListResponse
    {
        [JsonProperty("applist")]
        public AppList AppList { get; set; }
    }
}