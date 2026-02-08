using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using ApiClient.Marketstack.Services;
using System.Collections.Generic;
// using Serilog;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Marketstack API.
    /// </summary>
    public class MarketstackApi
    {
        private readonly string _baseUrl = "https://api.marketstack.com/v2";
        private readonly HttpClient _httpClient;
        private readonly ILogger? _logger;
        private readonly KeyValuePair<string, string> _requiredParams;

        internal MarketstackApi(
            HttpClient httpClient, string apiKey, ILogger? logger = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(apiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
            
            _requiredParams = new("access_key", apiKey);
            _httpClient = httpClient;
            _logger = logger;
        }

        public MarketstackApi(string apiKey, ILogger? logger = null)
            : this(new HttpClient(), apiKey, logger)
        {
        }
    
        /// <summary>
        /// Gets Eod data for the given symbols and date.
        /// </summary>
        /// <param name="symbols">Array of stock or bond tickers.</param>
        /// <param name="date">Date to fetch data for.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="EodResponse"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<EodResponse> GetEodResponseAsync(string[] symbols, DateTime date)
        {
            var queryBuilder = GetQueryBuilder();
            var symbolsDelimited = string.Join(',', symbols);

            queryBuilder.AddParameter("symbols", symbolsDelimited);
            queryBuilder.AddParameter("date", date.ToString("yyyy-MM-dd"));

            var uriBuilder = GetUriBuilder(endpoint: Endpoints.Eod);
            uriBuilder.Query = queryBuilder.ToString();

            var requestUrl = uriBuilder.Uri.AbsoluteUri;

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

        private QueryBuilder GetQueryBuilder() => new(initParameters: _requiredParams);

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
