using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using ApiClient.Marketstack.Services;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace ApiClient.Marketstack
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Marketstack API.
    /// </summary>
    public class MarketstackApi
    {
        private readonly string _baseUrl = "https://api.marketstack.com/v2";
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string[] endpoints = ["eod"];

        public MarketstackApi(string apiKey, ILogger? logger = null)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
        }
    
        /// <inheritdoc/>
        public async Task<EodResponse> GetEodDataAsync(string[] symbols, DateTime date)
        {
            var queryBuilder = new QueryBuilder();
            var symbolsDelimited = string.Join(',', symbols);

            queryBuilder.AddParameter("symbols", symbolsDelimited);
            queryBuilder.AddParameter("date", date, format: "yyyy-MM-dd");

            var uriBuilder = GetUriBuilder(endpoint: "eod");
            uriBuilder.Query = queryBuilder.ToString();

            var requestUrl = $"{_baseUrl}/eod?access_key={_apiKey}&symbols={symbolsDelimited}&date={date:yyyy-MM-dd}";

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

        private UriBuilder GetUriBuilder(string endpoint) => new(uri: $"{_baseUrl}/{endpoint}");
    }
}
