using System;

namespace GameMiner.BusinessLayer
{
    public interface ICoinhiveApiClient
    {
        double GetMegahashPayout();
        double GetExchangeRate();
        long GetUserBalance(string username);
        bool Withdraw(string username, long amount);
        bool VerifyToken(string token, long numberOfHashes);
    }
}