using GameMiner.BusinessLayer;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameMiner.DataLayer.Model
{
    public class Giveaway : BaseUserModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public GiveawayStatus Status { get; set; }
        public long GameId { get; set; }
        public string Description { get; set; }
        public virtual Game Game { get; set; }
        public virtual ICollection<GiveawayEntry> Entries { get; set; }
        public virtual ICollection<GiveawayWinner> GiveawayWinners { get; set; }
    }
}
