using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ApiClient.Services;
using ApiClient.Resources;
using ApiClient.Massive.Response.Stocks;
using Microsoft.Extensions.Configuration;

namespace ApiClient.Massive
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Massive API.
    /// </summary>
    public partial class MassiveApi : Services.ApiClient, IMassiveApi
    {
        private const string _baseUrl = "https://api.massive.com";
        private readonly Uri _baseUri = new(_baseUrl);
        private readonly short _maximumDateRangeInDays = 30;
        private readonly ILogger? _logger;
        private readonly RateTimer? _rateTimer;

        public MassiveApi(string apiKey, IConfiguration? configuration = null, ILogger? logger = null)
            : this(new HttpClient(), apiKey, configuration, logger)
        {
        }

        internal MassiveApi(
            HttpClient httpClient,
            string apiKey,
            IConfiguration? configuration = null,
            ILogger? logger = null)
            : base(
                baseUrl: _baseUrl,
                httpClient: httpClient
            )
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentException.ThrowIfNullOrEmpty(apiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

            RequiredParams.Add("apiKey", apiKey);
            RateOptions options = new();
            configuration?
                .GetSection("massive")
                .GetSection(nameof(RateOptions))
                .Bind(options);

            _rateTimer = new RateTimer(options.Limit, options.Interval);
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
            var absoluteUri = GetAbsoluteUri(endPoint);
            var uriBuilder = new UriBuilder(absoluteUri)
            {
                Query = queryBuilder.ToString()
            };
            var requestUrl = uriBuilder.Uri.AbsoluteUri;

            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

#if DEBUG
#pragma warning disable CA1873 // Avoid potentially expensive logging
                _logger?.LogDebug("{@headers}", response.Headers);
                _logger?.LogDebug("{@responseBody}", responseBody);
#pragma warning restore CA1873 // Avoid potentially expensive logging
#endif

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
            if (dateFrom > dateTo)
            {
                throw new ArgumentException(
                    $"Range invalid: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}");
            }
            if (dateTo.Subtract(dateFrom).Days > _maximumDateRangeInDays)
            {
                throw new ArgumentException(
                    $"Range too long: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}");
            }
            return true;
        }

        /// <summary>
        /// Gets the absolute <see cref="Uri"/> instance for the given relative endpoint.
        /// </summary>
        /// <param name="endpoint">Relative path to the endpoint.</param>
        /// <returns>A <see cref="Uri"/> where the URI is set to the absolute path of the endpoint.</returns>
        private Uri GetAbsoluteUri(string endpoint) => new(_baseUri, relativeUri: endpoint);

        /// <summary>
        /// Gets a <see cref="QueryBuilder"/> instance with required parameters initialized.
        /// </summary>
        /// <returns>A <see cref="QueryBuilder"/> configured for required parameters.</returns>
        private QueryBuilder GetQueryBuilder() => new(initParameters: RequiredParams);

        /// <summary>
        /// Collection of the relative endpoints for the api as stirng patterns.
        /// </summary>
        private readonly struct Endpoint
        {
            /// <summary>
            /// Handles stocks, options, indices.
            /// </summary>
            public const string TickerCustomBars = "/v2/aggs/ticker/{ticker}/range/{multiplier}/{timespan}/{from}/{to}";

            // TODO: Impement endpoint
            // public const string OptionsTickerSummary = "/v1/open-close/{optionsTicker}/{date}";

            // TODO: Impement endpoint
            // public const string OptionsPreviousDayBar = "/v2/aggs/ticker/{optionsTicker}/prev";

            public const string ReferenceAllTickers = "/v3/reference/tickers";

            public const string ReferenceTickerOverview = "/v3/reference/tickers/{ticker}";

            public const string ReferenceTickerTypes = "/v3/reference/tickers/types";

            // TODO: Impement endpoint
            // public const string StocksRelatedTickers = "/v1/related-companies/{ticker}";

            // TODO: Impement endpoint
            // public const string StocksDailySummary = "/v2/aggs/grouped/locale/us/market/stocks/{date}";

            public const string StocksFundamentalsShortVolume = "/stocks/v1/short-volume";

            // TODO: Impement endpoint
            // public const string StocksTickerSummary = "/v1/open-close/{stocksTicker}/{date}";

            // TODO: Impement endpoint
            // public const string StocksPreviousDayBar = "/v2/aggs/ticker/{stocksTicker}/prev";
        }
    }

    #region Private, generalized methods.
    public partial class MassiveApi
    {
        /// <summary>
        /// General-purpose method for retrieving <see cref="AggregateBarResponse"/> for stocks, options, and indexes.
        /// </summary>
        /// <param name="ticker">The ticker of the asset. Use patterns 
        /// <list><item>O:{ticker}, for options</item>
        /// <item>I:{ticker}, for indices</item>
        /// <item>{ticker}, for stocks</item></list></param>
        /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
        /// <param name="timeSpan">Size of the time window.</param>
        /// <param name="from">Start of the time window.</param>
        /// <param name="to">End of the time window.</param>
        /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
        private async Task<AggregateBarResponse> GetGenericAggregateBarResponseAsync(
            string ticker, 
            int multiplier, 
            BarTimespanEnum timeSpan, 
            DateTime from, 
            DateTime to, 
            int limit = 100)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(limit, 0, nameof(limit));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 1000, nameof(limit));
            
            string endpointPattern = QueryBuilder
                                        .ConvertEndpointToStringPattern(Endpoint.TickerCustomBars);

            string endpoint = string.Format(endpointPattern, 
                                    ticker, 
                                    multiplier, 
                                    timeSpan.ToString().ToLower(), 
                                    $"{from:yyyy-MM-dd}", 
                                    $"{to:yyyy-MM-dd}");

            var queryBuilder = GetQueryBuilder();
            queryBuilder.AddParameter("limit", $"{limit}");
            
            var response = await GetResponseAsync<AggregateBarResponse>(queryBuilder, endpoint);
            
            return response;
        }

        /// <summary>
        /// Retrieve comprehensive details for a single ticker supported by Massive that is active as-of a given date.
        /// </summary>
        /// <param name="ticker">Filter by a ticker symbol.Use patterns 
        /// <list><item>O:{ticker}, for options</item>
        /// <item>I:{ticker}, for indices</item>
        /// <item>{ticker}, for stocks</item></list></param>
        /// <param name="date">Specify a point in time to retrieve tickers available on that date. Defaults to the most recent available date.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="TickerOverviewResponse"/>.</returns>
        private async Task<TickerOverviewResponse> GetGenericTickerOverviewResponseAsync(
            string ticker,
            DateTime? date = null
        )
        {
            ArgumentException.ThrowIfNullOrEmpty(ticker);

            string endpointPattern = QueryBuilder
                .ConvertEndpointToStringPattern(Endpoint.ReferenceTickerOverview);

            string endpoint = string.Format(endpointPattern, ticker);

            var queryBuilder = GetQueryBuilder();
            if(date is not null)
                queryBuilder.AddParameter("date", $"{date:yyyy-MM-dd}");

            var response = await GetResponseAsync<TickerOverviewResponse>(queryBuilder, endpoint);

            return response;
        }
    }
    #endregion
}