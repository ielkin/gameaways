using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public interface IGiveawayService
    {
        Task PickWinnersAsync(int giveawayId);
    }
}
