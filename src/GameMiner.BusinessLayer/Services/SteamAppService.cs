using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GameMiner.BusinessLayer.Steam.Entities;
using GameMiner.BusinessLayer.Steam.Response;
using GameMiner.Core;
using Newtonsoft.Json;

namespace GameMiner.BusinessLayer.Steam
{
    public class SteamAppService : BaseHttpApiClient, ISteamAppService
    {
        private string _getAppDataUrl = "http://store.steampowered.com/api/appdetails?appids={0}";
        private string _getAppListUrl = "http://api.steampowered.com/ISteamApps/GetAppList/v0002/";

        public SteamAppService()
            : base(new JsonService())
        {
        }

        public async Task<KeyValuePair<long, SteamAppDataResponse>> GetAppDetailsAsync(long appId)
        {
            string countryCode = "US";

            string url = string.Format(_getAppDataUrl, appId, countryCode);

            var appDataResponse = await _jsonService.GetAsync<IDictionary<long, SteamAppDataResponse>>(url);

            return appDataResponse.First();
        }

        public async Task<IList<SteamApp>> GetAppList()
        {
            string url = _getAppListUrl;

            var appDataResponse = await _jsonService.GetAsync<GetAppListResponse>(url);

            return appDataResponse.AppList.Apps;
        }
    }
}
