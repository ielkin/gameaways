using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    public class PriceInformation
    {
        [JsonProperty("initial")]
        public int InitialPrice { get; set; }
    }
}