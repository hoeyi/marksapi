using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using ApiClient.Marketstack.Services;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Marketstack API.
    /// </summary>
    public class MarketstackApi
    {
        private readonly string _baseUrl = "https://api.marketstack.com/v2";
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger? _logger;

#pragma warning disable IDE0290 // Use primary constructor
        public MarketstackApi(string apiKey, ILogger? logger = null)
#pragma warning restore IDE0290 // Use primary constructor
        {
            ArgumentException.ThrowIfNullOrEmpty(apiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
            
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _logger = logger;
        }
    
        /// <inheritdoc/>
        public async Task<EodResponse> GetEodDataAsync(string[] symbols, DateTime date)
        {
            var queryBuilder = new QueryBuilder();
            var symbolsDelimited = string.Join(',', symbols);

            queryBuilder.AddParameter("symbols", symbolsDelimited);
            queryBuilder.AddParameter("date", date, format: "yyyy-MM-dd");

            var uriBuilder = GetUriBuilder(endpoint: Endpoints.Eod);
            uriBuilder.Query = queryBuilder.ToString();

            var requestUrl = $"{_baseUrl}/eod?access_key={_apiKey}&symbols={symbolsDelimited}&date={date:yyyy-MM-dd}";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response. If the response is null thow invalid operation
                EodResponse eodResponse = JsonConvert
                    .DeserializeObject<EodResponse>(responseBody) ?? 
                    throw new InvalidOperationException(message: LoggingTemplates.Error.InvalidOrEmptyResponse);

                return eodResponse;
            }
            catch (HttpRequestException e)
            {
                _logger?.LogError(LoggingTemplates.Error.HttpErrorGeneral, e);
                throw;
            }
        }

        private UriBuilder GetUriBuilder(string endpoint) => new(uri: $"{_baseUrl}/{endpoint}");

        /// <summary>
        /// Collection of the relative endpoints for the api as stirng patterns.
        /// </summary>
        private class Endpoints
        {
            public const string Eod = "eod";
            public const string EodLatest  = "eod/latest ";
            public const string EodByDatePattern = "eod/{0} ";
            public const string Intraday  = "intraday ";
            public const string IntradayLatest  = "intraday/latest ";
            public const string IntradayByDate = "intraday/{0} ";
            public const string Stockprice  = "stockprice ";
            public const string Commodities  = "commodities ";
            public const string Commoditieshistory  = "commoditieshistory ";
            public const string Companyratings  = "companyratings ";
            public const string Splits  = "splits ";
            public const string Dividends  = "dividends ";
            public const string TickersBySymbol = "tickers/{0} ";
            public const string TickersBySymbolEod  = "tickers/{0}/eod ";
            public const string TickersBySymbolSplits  = "tickers/{0}/splits ";
            public const string TickersBySymbolDividends  = "tickers/{0}/dividends ";
            public const string TickersBySymbolIntraday  = "tickers/{0}/intraday ";
            public const string TickersBySymbolEodByDate = "tickers/{0}/eod/{date} ";
            public const string TickersBySymbolEodLatest  = "tickers/{0}/eod/latest ";
            public const string TickersBySymbolIntradayLatest  = "tickers/{0}/intraday/latest ";
            public const string Tickerslist  = "tickerslist ";
            public const string Tickerinfo  = "tickerinfo ";
            public const string Indexlist  = "indexlist ";
            public const string Indexinfo  = "indexinfo ";
            public const string Exchanges  = "exchanges ";
            public const string ExchangesByMic = "exchanges/{0} ";
            public const string ExchangesByMicTickers  = "exchanges/{0}/tickers ";
            public const string ExchangesByMicEod  = "exchanges/{0}/eod ";
            public const string ExchangesByMicEodLatest  = "exchanges/{0}/eod/latest ";
            public const string ExchangesByMicEodByDate = "exchanges/{0}/eod/{1} ";
            public const string ExchangesByMicIntraday  = "exchanges/{0}/intraday ";
            public const string ExchangesByMicIntradayLatest  = "exchanges/{0}/intraday/latest ";
            public const string ExchangesByMicIntradayByDate = "exchanges/{0}/intraday/{1} ";
            public const string Currencies  = "currencies ";
            public const string Timezones  = "timezones ";
            public const string Bondlist  = "bondlist ";
            public const string Bond  = "bond ";
            public const string Etflist  = "etflist ";
            public const string Etfholdings  = "etfholdings ";
            public const string Cik_code  = "cik_code ";
            public const string Company_name  = "company_name ";
            public const string Submissions  = "submissions ";
            public const string Company_facts  = "company_facts ";
        }
    }
}
