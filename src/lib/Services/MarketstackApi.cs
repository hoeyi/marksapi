using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Marketstack API.
    /// </summary>
    public class MarketstackApi
    {
        private readonly string _baseUrl = "https://api.marketstack.com/v2/eod";
        private readonly HttpClient _httpClient;
        private readonly IQueryBuilder queryBuilder;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string[] endpoints = [];

        public MarketstackApi(string apiKey)
        {
            _apiKey = apiKey;
            _baseUrl = ;
            _httpClient = new HttpClient();
        }
        
        /// <inheritdoc/>
        public async Task<EodResponse> GetEodDataAsync(string symbol, DateTime date)
        {
            var requestUrl = $"{_baseUrl}?access_key={_apiKey}&symbols={symbol}&date={date:yyyy-MM-dd}";

            try
            {
                using var httpClient = HttpFactory.CreateClient();

                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
                EodResponse eodResponse = JsonConvert.DeserializeObject<EodResponse>(responseBody);
                return eodResponse;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }
        }

        /// <summary>
        /// Helper class for appending parameters to an API endpoint URL.
        /// </summary>        
        class QueryBuilder
        {
            public string BuildQuery(params KeyValuePair<string, string>[] queryParameters)
            {
                var queryString = new StringBuilder("?");

                foreach (var queryParameter in queryParameters)
                {
                    if (queryParameter.Value != string.Empty)
                    {
                        queryString.Append(queryParameter.Key.ToLower() + "=" + queryParameter.Value + "&");
                    }
                }

                return queryString.ToString().TrimEnd('&');
            }
        }
    }
}
