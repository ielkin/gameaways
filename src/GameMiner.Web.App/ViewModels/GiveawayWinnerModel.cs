using System;
using GameMiner.BusinessLayer;

namespace Gameaways.Web.Model
{
    public class GiveawayWinnerModel : BaseUserViewModel
    {
        public GiftStatus GiftStatus { get; set; }
        public string RedemptionCode { get; set; }
        public DateTime LastUpdated { get; set; }
        public string SteamProfileLink { get; set; }
    }
}