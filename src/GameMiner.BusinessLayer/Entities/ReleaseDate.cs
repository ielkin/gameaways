using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam.Entities
{
    public class ReleaseDate
    {
        [JsonProperty("coming_soon")]
        public bool ComingSoon { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
    }
}