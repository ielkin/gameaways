using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public interface IHttpService
    {
        T Get<T>(string url);
        Task<T> GetAsync<T>(string url);
        Task<T> GetAsync<T>(string url, IDictionary<string, string> headers);
    }
}