using System;
using System.Collections;
using System.Collections.Generic;

namespace GameMiner.DataLayer.Model
{
    public class GiveawayEntry : BaseUserModel
    {
        public long GiveawayId { get; set; }
        public DateTime EntryDate { get; set; }

        public virtual Giveaway Giveaway { get; set; }
    }
}