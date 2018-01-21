using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace GameMiner.BusinessLayer
{
    public class CoinhiveApiClient : ICoinhiveApiClient
    {
        private readonly string _baseUrl = "https://api.coinhive.com";
        private readonly string _serverSecret;

        public CoinhiveApiClient(IConfiguration configuration)
        {
            _serverSecret = configuration["CoinHive:ServerSecret"];
        }

        public double GetMegahashPayout()
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(_baseUrl) })
            {
                string responseString = httpClient.GetStringAsync($"/stats/payout?secret={ _serverSecret }").Result;

                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                return (double)responseObject.payoutPer1MHashes;
            }
        }

        public double GetExchangeRate()
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(_baseUrl) })
            {
                string responseString = httpClient.GetStringAsync($"/stats/payout?secret={ _serverSecret }").Result;

                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                return responseObject.xmrToUsd;
            }
        }

        public long GetUserBalance(string username)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(_baseUrl) })
            {
                string responseString = httpClient.GetStringAsync($"/user/balance?name={ username }&secret={ _serverSecret }").Result;

                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                if (responseObject.success == true)
                {
                    return responseObject.balance;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool Withdraw(string username, long amount)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(_baseUrl) })
            {
                IList<KeyValuePair<string, string>> formParams = new List<KeyValuePair<string, string>>();

                formParams.Add(new KeyValuePair<string, string>("name", username));
                formParams.Add(new KeyValuePair<string, string>("amount", amount.ToString()));
                formParams.Add(new KeyValuePair<string, string>("secret", _serverSecret));

                var requestContent = new FormUrlEncodedContent(formParams);

                var httpResponse = httpClient.PostAsync("/user/withdraw", requestContent).Result;

                var responseString = httpResponse.Content.ReadAsStringAsync().Result;

                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                return responseObject.success;
            }
        }

        public bool VerifyToken(string token, long numberOfHashes)
        {
            using (var httpClient = new HttpClient() { BaseAddress = new Uri(_baseUrl) })
            {
                IList<KeyValuePair<string, string>> formParams = new List<KeyValuePair<string, string>>();

                formParams.Add(new KeyValuePair<string, string>("token", token));
                formParams.Add(new KeyValuePair<string, string>("hashes", numberOfHashes.ToString()));
                formParams.Add(new KeyValuePair<string, string>("secret", _serverSecret));

                var requestContent = new FormUrlEncodedContent(formParams);

                var httpResponse = httpClient.PostAsync("/token/verify", requestContent).Result;

                var responseString = httpResponse.Content.ReadAsStringAsync().Result;

                dynamic responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

                return responseObject.success;
            }
        }
    }
}
