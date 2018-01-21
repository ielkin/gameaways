using System;
using System.Collections.Generic;
using System.Linq;
using GameMiner.BusinessLayer;
using GameMiner.BusinessLayer.Core;

namespace Gameaways.Web.Model
{
    public class GiveawayViewModel : BaseUserViewModel
    {
        public long GameId { get; set; }
        public string SteamAppId { get; set; }
        public string Title { get; set; }
        public int FundingProgress { get; set; }
        public int EntriesCount { get; set; }
        public int CurrentUserEntriesCount { get; set; }
        public string Description { get; set; }
        public string TileUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GiveawayStatus Status { get; set; }
        public int NumberOfCopies { get; set; }
        public EntryStatus EntryStatus { get; set; }
    }
}