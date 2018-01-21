using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMiner.BusinessLayer.Steam.Response;
using Newtonsoft.Json;
using GameMiner.BusinessLayer.Steam.Entities;

namespace GameMiner.BusinessLayer.Steam
{
    public class SteamUserService : BaseHttpApiClient
    {
        private string _playerSummaryApiUrl = "https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=82A42B326DF37A3180E720AD4C9A7AD6&steamids={0}";

        public SteamUserService()
            : base(new JsonService())
        {
        }

        public async Task<PlayerSummary> GetPlayerSummaryAsync(long steamId)
        {
            string url = string.Format(_playerSummaryApiUrl, steamId);

            var playerSummariesObject = await _jsonService.GetAsync<GetPlayerSummariesObject>(url);

            return playerSummariesObject.Response.Players[0];
        }
    }
}
