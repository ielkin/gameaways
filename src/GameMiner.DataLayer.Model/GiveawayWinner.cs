using System;
using System.Collections;
using System.Collections.Generic;
using GameMiner.BusinessLayer;

namespace GameMiner.DataLayer.Model
{
    public class GiveawayWinner : BaseUserModel
    {
        public long GiveawayId { get; set; }

        public GiftStatus GiftStatus { get; set; }

        public DateTime LastUpdated { get; set; }

        public string RedemptionCode { get; set; }

        public virtual Giveaway Giveaway { get; set; }
    }
}