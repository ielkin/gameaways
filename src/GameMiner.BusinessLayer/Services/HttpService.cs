using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameMiner.BusinessLayer
{
    public abstract class HttpService : IHttpService
    {
        private int timeout = 300000;
        private ISerializer _serializer;

        public HttpService(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public T Get<T>(string url)
        {
            var task = GetAsync<T>(url);

            return task.Result;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            return await GetAsync<T>(url, new Dictionary<string, string>());
        }

        public async Task<T> GetAsync<T>(string url, IDictionary<string, string> headers)
        {
            string responseString = string.Empty;

            using (HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromMilliseconds(timeout) })
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                responseString = await httpClient.GetStringAsync(url);
            }

            return _serializer.Deserialize<T>(responseString);
        }
    }
}
