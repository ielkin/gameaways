using System;
using System.Collections;
using System.Collections.Generic;

namespace GameMiner.DataLayer.Model
{
    public class ExcludedSteamGame : BaseModel
    {
        public long SteamAppId { get; set; }
    }
}