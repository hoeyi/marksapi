using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using ApiClient.Marketstack.Services;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Marketstack API.
    /// </summary>
    public partial class MarketstackApi
    {
        private readonly string _baseUrl = "https://api.marketstack.com/v2";
        private readonly short _maximumDateRangeInDays = 30;
        private readonly HttpClient _httpClient;
        private readonly ILogger? _logger;
        private readonly KeyValuePair<string, string> _requiredParams;

        public MarketstackApi(string apiKey, ILogger? logger = null)
            : this(new HttpClient(), apiKey, logger)
        {
        }

        internal MarketstackApi(
            HttpClient httpClient, string apiKey, ILogger? logger = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(apiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
            
            _requiredParams = new("access_key", apiKey);
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Posts a GET request from the given <see cref="QueryBuilder"/> and <see cref="Endpoint"/>. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryBuilder"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The response body was empty.</exception>
        internal async Task<T> GetResponseAsync<T>(QueryBuilder queryBuilder, string endPoint)
        {
            var uriBuilder = GetUriBuilder(endPoint);
            uriBuilder.Query = queryBuilder.ToString();

            var requestUrl = uriBuilder.Uri.AbsoluteUri;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Parse the JSON response. If the response is null thow invalid operation
                T genericResponse = JsonConvert
                    .DeserializeObject<T>(responseBody) ?? 
                    throw new InvalidOperationException(message: LoggingTemplates.Error.InvalidOrEmptyResponse);

                return genericResponse;
            }
            catch (HttpRequestException e)
            {
                _logger?.LogError(LoggingTemplates.Error.HttpErrorGeneral, e);
                throw;
            }
        }

        /// <summary>
        /// Validates the given dates form an acceptable date range parameter.
        /// </summary>
        /// <param name="dateFrom">Start date of the range tested.</param>
        /// <param name="dateTo">End date of the range tested.</param>
        /// <returns>Return <see cref="True"/> if the range is acceptable, else throw <see cref="ArgumentException"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="dateFrom"/> is greater than <paramref name="dateTo"/> or the 
        /// range measured in days is too long.</exception>
        internal bool ValidateDateRangeOrThrow(DateTime dateFrom, DateTime dateTo)
        {
            if(dateFrom > dateTo)
            {
                throw new ArgumentException(
                    $"Range invalid: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}");
            }
            if(dateTo.Subtract(dateFrom).Days > _maximumDateRangeInDays)
            {
                throw new ArgumentException(
                    $"Range too long: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}");
            }
            return true;
        }

        /// <summary>
        /// Gets a <see cref="UriBuilder"/> instance from the given relative endpoint.
        /// </summary>
        /// <param name="endpoint">Relative path to the endpoint.</param>
        /// <returns>A <see cref="UriBuilder"/> where the URI is set to the absolute path of the endpoint.</returns>
        private UriBuilder GetUriBuilder(string endpoint) => new(uri: $"{_baseUrl}/{endpoint}");

        /// <summary>
        /// Gets a <see cref="QueryBuilder"/> instance with required parameters initialized.
        /// </summary>
        /// <returns>A <see cref="QueryBuilder"/> configured for required parameters.</returns>
        private QueryBuilder GetQueryBuilder() => new(initParameters: _requiredParams);

        /// <summary>
        /// Collection of the relative endpoints for the api as stirng patterns.
        /// </summary>
        private class Endpoint
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

    #region Endpoints: { /eod, /intraday }
    public partial class MarketstackApi
    {
        /// <summary>
        /// Gets Eod data for the given symbols and date range.
        /// </summary>
        /// <param name="symbols">Array of stock or bond tickers.</param>
        /// <param name="dateFrom">Start date of the query range.</param>
        /// <param name="dateTo">End date of the query range.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="EodResponse"/>.</returns>
        public async Task<EodResponse> GetEodResponseAsync(
                    string[] symbols, DateTime dateFrom, DateTime dateTo)
        {
            

            var queryBuilder = GetQueryBuilder();
            var symbolsDelimited = string.Join(',', symbols);

            queryBuilder.AddParameter("symbols", symbolsDelimited);
            queryBuilder.AddParameter("date_from", dateFrom.ToString("yyyy-MM-dd"));
            queryBuilder.AddParameter("date_to", dateTo.ToString("yyyy-MM-dd"));

            var response = await GetResponseAsync<EodResponse>(queryBuilder, Endpoint.Eod);

            return response;
        }

        /// <summary>
        /// Gets Eod data for the given symbols and date.
        /// </summary>
        /// <param name="symbols">Array of stock or bond tickers.</param>
        /// <param name="date">Date to fetch data for.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="EodResponse"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<EodResponse> GetEodResponseAsync(string[] symbols, DateTime date)
            => await GetEodResponseAsync(symbols: symbols, dateFrom: date, dateTo: date);

        /// <summary>
        /// Gets Intraday data for the given symbols and date.
        /// </summary>
        /// <param name="symbols">Symbols to query quotes for.</param>
        /// <param name="dateFrom">Start date of the range to query.</param>
        /// <param name="dateTo">End date of the range to query.</param>
        /// <param name="afterHours">Flag to include after-hours data in query.</param>
        /// <param name="exchangeMic">Filters results for the exchange with the given MIC.</param>
        /// <param name="interval">Timing interval to query. One of {"1min", "5min", "10min", "15min", "30min", "1hour"}.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="IntradayResponse"/>.</returns>
        public async Task<IntradayResponse> GetIntradayResponseAsync(
            string[] symbols, DateTime date, bool afterHours = false, string? exchangeMic = null, string interval = "15min") 
            => await GetIntradayResponseAsync(
                symbols, date, date, afterHours, exchangeMic, interval);

        /// <summary>
        /// Gets Intraday data for the given symbols and date range.
        /// </summary>
        /// <param name="symbols">Symbols to query quotes for.</param>
        /// <param name="dateFrom">Start date of the range to query.</param>
        /// <param name="dateTo">End date of the range to query.</param>
        /// <param name="afterHours">Flag to include after-hours data in query.</param>
        /// <param name="exchangeMic">Filters results for the exchange with the given MIC.</param>
        /// <param name="interval">Timing interval to query. One of {"1min", "5min", "10min", "15min", "30min", "1hour"}.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="IntradayResponse"/>.</returns>
        public async Task<IntradayResponse> GetIntradayResponseAsync(
            string[] symbols, 
            DateTime dateFrom, 
            DateTime dateTo, 
            bool afterHours = false, 
            string? exchangeMic = null, 
            string interval = "15min")
        {
            var queryBuilder = GetQueryBuilder();
            var symbolsDelimited = string.Join(',', symbols);

            queryBuilder.AddParameter("symbols", symbolsDelimited);
            queryBuilder.AddParameter("date_from", dateFrom.ToString("yyyy-MM-dd"));
            queryBuilder.AddParameter("date_to", dateTo.ToString("yyyy-MM-dd"));
            
            // Optional parameters
            if(afterHours) 
                queryBuilder.AddParameter("after_hours", afterHours.ToString());
            if(!string.IsNullOrEmpty(exchangeMic)) 
                queryBuilder.AddParameter("exchange_mic", exchangeMic);
            
            queryBuilder.AddParameter("interval", interval);

            var response = await GetResponseAsync<IntradayResponse>(queryBuilder, Endpoint.Intraday);

            return response;
        }
    }
    #endregion
}
