using System;
using System.Collections.Generic;

namespace GameMiner.DataLayer.Model
{
    public class Game : BaseModel
    {
        public string Title { get; set; }
        public string LargeHeaderImageUrl { get; set; }
        public string HeaderImageUrl { get; set; }
        public string SmallHeaderImageUrl { get; set; }
        public long SteamAppId { get; set; }
        public double Price { get; set; }
    }
}
