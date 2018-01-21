using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using GameMiner.Core;
using GameMiner.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace GameMiner.BusinessLayer
{
    public class GiveawayService : IGiveawayService
    {
        private IRandomizationService _randomizationService;
        private GameawaysDbContext _db;

        public GiveawayService(GameawaysDbContext db)
        {
            _randomizationService = new DotNetRandomizer();
            _db = db;
        }

        public async Task SendGiftAsync(int winnerId, string redemptionCode)
        {
            var winner = _db.GiveawayWinners.FirstOrDefault(ge => ge.Id == winnerId);

            winner.GiftStatus = GiftStatus.Sent;
            winner.RedemptionCode = redemptionCode;

            await _db.SaveChangesAsync();
        }

        public async Task PickNewWinnerAsync(int winnerId)
        {
            //var winner = _db.GiveawayWinners.FirstOrDefault(ge => ge.Id == winnerId);

            //var giveaway = winner.Giveaway;

            //long[] winnerUserIds = giveaway.GiveawayWinners.Select(gw => gw.UserId).ToArray();

            //while (true)
            //{
            //    var integers = _randomizationService.GenerateRandomIntegers(0, giveaway.Entries.Count() - 1, 1);

            //    int winnerIndex = integers[0];
            //    int winnerUserId = giveaway.Entries.ToArray()[winnerIndex].UserId;

            //    if (!winnerUserIds.Contains(winnerUserId))
            //    {
            //        winner.UserId = winnerUserId;
            //        winner.LastUpdated = DateTime.UtcNow;

            //        await _db.SaveChangesAsync();

            //        _db.Notifications.Add(new Notification()
            //        {
            //            NotificationDate = DateTime.UtcNow,
            //            Status = NotificationStatus.New,
            //            Text = "Congratulations!!! You have won a giveaway!",
            //            Url = "/Giveaway/" + winner.GiveawayId,
            //            UserId = winner.UserId,
            //        });

            //        await _db.SaveChangesAsync();

            //        break;
            //    }
            //}
        }

        public async Task PickWinnersAsync(int giveawayId)
        {
            var giveaway = await _db.Giveaways.FirstAsync(ga => ga.Id == giveawayId);
            var entries = await _db.GiveawayEntries.Where(ge => ge.GiveawayId == giveawayId).ToListAsync();

            //if (giveaway.Status != GiveawayStatus.WinnersPicked)
            //{
            //    int uniqueEntrants = entries.Select(e => e.UserId).Distinct().Count();

            //    List<int> winnerUserIds = new List<int>();

            //    for (int i = 0; i < nuberOfPrizes; i++)
            //    {
            //        int winnerIndex = 0;

            //        if (uniqueEntrants > nuberOfPrizes)
            //        {
            //            winnerIndex = _randomizationService.GenerateRandomIntegers(0, entries.Count() - 1, 1)[0];
            //        }
            //        else
            //        {
            //            winnerIndex = i;
            //        }

            //        int winnerUserId = entries[winnerIndex].UserId;

            //        if (winnerUserIds.Contains(winnerUserId))
            //        {
            //            i--;

            //            continue;
            //        }

            //        winnerUserIds.Add(winnerUserId);

            //        var winner = new GiveawayWinner()
            //        {
            //            GiveawayId = giveawayId,
            //            UserId = winnerUserId,
            //            GiftStatus = GiftStatus.NotSent,
            //            LastUpdated = DateTime.UtcNow,
            //        };

            //        if (redeemables != null && redeemables.Count() > i)
            //        {
            //            winner.RedemptionCode = redeemables[i];
            //            winner.GiftStatus = GiftStatus.Sent;
            //        }

            //        _db.GiveawayWinners.Add(winner);

            //        await _db.SaveChangesAsync();

            //        _db.Notifications.Add(new Notification()
            //        {
            //            NotificationDate = DateTime.UtcNow,
            //            Status = NotificationStatus.New,
            //            Text = "Congratulations!!! You have won a giveaway!",
            //            Url = "/Giveaway/" + winner.GiveawayId,
            //            UserId = winner.UserId,
            //        });

            //        await _db.SaveChangesAsync();
            //    }

            //    giveaway.Status = GiveawayStatus.WinnersPicked;

            //    _db.Notifications.Add(new Notification()
            //    {
            //        NotificationDate = DateTime.UtcNow,
            //        Status = NotificationStatus.New,
            //        Text = "Your giveaway has ended. Please send out the keys or prizes.",
            //        Url = "/Giveaway/" + giveaway.Id,
            //        UserId = giveaway.UserId,
            //    });

            //    await _db.SaveChangesAsync();
            //}
        }
    }
}
