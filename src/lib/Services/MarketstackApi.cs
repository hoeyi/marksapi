using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
        private readonly QueryBuilder queryBuilder;
        private readonly string _apiKey;
        private readonly string[] endpoints = [];

        public MarketstackApi(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }
        
        /// <inheritdoc/>
        public async Task<EodResponse> GetEodDataAsync(string symbol, DateTime date)
        {
            var requestUrl = $"{_baseUrl}?access_key={_apiKey}&symbols={symbol}&date={date:yyyy-MM-dd}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                EodResponse eodResponse = JsonConvert.DeserializeObject<EodResponse>(responseBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
                return eodResponse;
#pragma warning restore CS8603 // Possible null reference return.
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
            public static string BuildQuery(params KeyValuePair<string, string>[] queryParameters)
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
