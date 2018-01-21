using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GameMiner.BusinessLayer.Steam.Entities;

namespace GameMiner.BusinessLayer.Steam.Response
{
    public class GetPlayerSummariesResponse
    {
        [JsonProperty("players")]
        public IList<PlayerSummary> Players { get; set; }
    }
}
