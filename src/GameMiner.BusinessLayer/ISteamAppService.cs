using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameMiner.BusinessLayer.Steam.Entities;

namespace GameMiner.BusinessLayer.Steam
{
    public interface ISteamAppService
    {
        Task<IList<SteamApp>> GetAppList();
        Task<KeyValuePair<long, SteamAppDataResponse>> GetAppDetailsAsync(long appId);
    }
}