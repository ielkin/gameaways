using System;

namespace GameMiner.BusinessLayer
{
    public enum GiveawayStatus : byte
    {
        Open = 0,
        Ended = 40,
        WinnersPicked = 50,
        PrizesSent = 60,
        FundingFailed = 70,
    }
}
