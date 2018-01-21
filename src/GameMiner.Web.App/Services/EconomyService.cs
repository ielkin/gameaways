using GameMiner.BusinessLayer;
using GameMiner.DataLayer.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMiner.Web.App.Services
{
    public class EconomyService : IEconomyService
    {
        private readonly ICoinhiveApiClient _coinhiveApiClient;
        private readonly IMemoryCache _memoryCache;
        private readonly GameawaysDbContext _db;
        private readonly double _xmrToUsd;
        private readonly double _payoutPer1MHashes;
        private readonly long _hashesPerCredit;

        public EconomyService(ICoinhiveApiClient coinhiveApiClient, IMemoryCache memoryCache, GameawaysDbContext db)
        {
            _coinhiveApiClient = coinhiveApiClient;
            _memoryCache = memoryCache;
            _db = db;

            _xmrToUsd = _memoryCache.GetOrCreate<double>("XMR-USD", entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));

                return _coinhiveApiClient.GetExchangeRate();
            });

            _payoutPer1MHashes = _memoryCache.GetOrCreate<double>("Megahash-Payout", entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromHours(1));

                return _coinhiveApiClient.GetMegahashPayout();
            });

            _hashesPerCredit = (long)(1000 / (_payoutPer1MHashes / 1000 * _xmrToUsd * 100 * 0.75) / 40);
        }

        public long GetHashesPerCredit()
        {
            return _hashesPerCredit;
        }

        public long UpdateUserBalance(long userId)
        {
            var user = _db.Users.First(u => u.Id == userId);

            long hashes = _memoryCache.GetOrCreate<long>($"Hashes-{ user.UserName }", entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                return _coinhiveApiClient.GetUserBalance(user.UserName);
            });

            double credits = hashes / _hashesPerCredit;
            bool hashesWithdrawalResult = _coinhiveApiClient.Withdraw(user.UserName, (long)credits * _hashesPerCredit);

            if (credits >= 1 && hashesWithdrawalResult)
            {
                user.CreditBalance += (long)credits;

                _db.SaveChanges();
            }

            return user.CreditBalance;
        }
    }
}
