using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using GameMiner.BusinessLayer.Steam;
using GameMiner.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using GameMiner.Core;

namespace GameMiner.AzureJobs
{
    public class Functions
    {
        public static void UpdateSteamApps([QueueTrigger("update-steam-apps")] string message, TextWriter log)
        {
            log.WriteLine("UpdateSteamApps job started");

            ISteamAppService _appService = new SteamAppService();

            DbContextOptionsBuilder<GameawaysDbContext> builder = new DbContextOptionsBuilder<GameawaysDbContext>();

            builder.UseSqlServer(ConfigurationManager.ConnectionStrings["GameMinerDb"].ConnectionString);

            GameawaysDbContext _db = new GameawaysDbContext(builder.Options);

            var appsList = _appService.GetAppList().Result;

            DateTime updateDate = DateTime.UtcNow.AddDays(-1);

            var existingGameIds = _db.Games
                    .Select(sg => sg.SteamAppId)
                    .ToList();

            var excludedGameIds = _db.ExcludedStoreGames
                .Select(esg => esg.SteamAppId)
                .ToList();

            var newAppIds = appsList
                 .Select(a => a.Id.Value)
                 .Except(existingGameIds)
                 .Except(excludedGameIds)
                 .ToList();

            for (int i = 0; i < newAppIds.Count(); i++)
            {
                log.WriteLine("Steam App Id: " + newAppIds[i]);

                var app = _appService.GetAppDetailsAsync(newAppIds[i]).Result;

                if (app.Value.Success == false || app.Key != app.Value.AppData.Id || !(app.Value.AppData.ApplicationType == StoreApplicationType.Game ||
                    app.Value.AppData.ApplicationType == StoreApplicationType.Hardware ||
                    app.Value.AppData.ApplicationType == StoreApplicationType.Mod ||
                    app.Value.AppData.ApplicationType == StoreApplicationType.Video ||
                    app.Value.AppData.ApplicationType == StoreApplicationType.Series ||
                    app.Value.AppData.ApplicationType == StoreApplicationType.Episode ||
                    app.Value.AppData.ApplicationType == StoreApplicationType.Dlc))
                {
                    log.WriteLine("Get Steam App Failed: " + newAppIds[i]);

                    if (!excludedGameIds.Any(esgId => esgId == newAppIds[i]))
                    {
                        _db.ExcludedStoreGames.Add(new ExcludedSteamGame()
                        {
                            SteamAppId = newAppIds[i],
                        });

                        _db.SaveChanges();
                    }
                }
                else if (!_db.Games.Any(g => g.SteamAppId == app.Value.AppData.Id) && app.Value.AppData.IsFree == false && app.Value.AppData.PriceInformation != null)
                {
                    log.WriteLine("Steam App Valid: " + newAppIds[i]);
                    log.WriteLine("Steam App Type: " + app.Value.AppData.ApplicationType);

                    var game = new Game()
                    {
                        Title = app.Value.AppData.Name,
                        HeaderImageUrl = app.Value.AppData.LargeTileUrl,
                        SmallHeaderImageUrl = string.Format("http://cdn.akamai.steamstatic.com/steam/apps/{0}/capsule_184x69.jpg", app.Value.AppData.Id),
                        SteamAppId = app.Value.AppData.Id,
                        Price = app.Value.AppData.PriceInformation.InitialPrice,
                    };

                    _db.Games.Add(game);

                    _db.SaveChanges();
                }

                System.Threading.Thread.Sleep(1500);
            }
        }
    }
}
