using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ApiClient.Services;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Massive.Response;
using System.Threading;
using System.Net.Http.Headers;
using static ApiClient.Services.RateTimer;

namespace ApiClient.Massive
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Massive API.
    /// </summary>
    public partial class MassiveApi : Services.ApiClient, IMassiveApi, IDisposable
    {
        /// <summary>
        /// Collection of the relative endpoints for the api as stirng patterns.
        /// </summary>
        private readonly struct Endpoint
        {
            public const string TreasuryYields = "/fed/v1/treasury-yields";
            
            public const string Inflation = "/fed/v1/inflation";

            public const string InflationExpectations = "/fed/v1/inflation-expectations";

            public const string LaborMarket = "/fed/v1/labor-market";

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

            public const string ReferenceContractOverview = "/v3/reference/options/contracts/{options_ticker}";

            public const string ReferenceTickerTypes = "/v3/reference/tickers/types";

            // TODO: Impement endpoint
            // public const string StocksRelatedTickers = "/v1/related-companies/{ticker}";

            // TODO: Impement endpoint
            // public const string StocksDailySummary = "/v2/aggs/grouped/locale/us/market/stocks/{date}";

            public const string StocksFundamentalsShortVolume = "/stocks/v1/short-volume";

            public const string StocksFundamentalsShortInterest = "/stocks/v1/short-interest";
            
            // TODO: Impement endpoint
            // public const string StocksTickerSummary = "/v1/open-close/{stocksTicker}/{date}";

            // TODO: Impement endpoint
            // public const string StocksPreviousDayBar = "/v2/aggs/ticker/{stocksTicker}/prev";
        }

        private const string _baseUrl = "https://api.massive.com";
        private readonly Uri _baseUri = new(_baseUrl);
        private readonly short _maximumDateRangeInDays = 30;
        private readonly ILogger? _logger;
        private readonly RateTimer? _rateTimer;

        /// <summary>
        /// Creates a new instance of <see cref="MassiveApi"/>.
        /// </summary>
        /// <param name="apiKey">API key for Massive authentication.</param>
        /// <param name="rateOptions">The <see cref="RateOptions"/> instance for this client.</param>
        /// <param name="logger">The <see cref="ILogger"/> instance for this client.</param>
        public MassiveApi(string apiKey, RateOptions? rateOptions = null, ILogger? logger = null)
            : this(new HttpClient(), apiKey, rateOptions, logger)
        {
        }

        internal MassiveApi(
            HttpClient httpClient,
            string apiKey,
            RateOptions? rateOptions = null,
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

            RateOptions options = rateOptions ?? new()
            {
                Limit = 5,
                Interval = 60
            };

            _logger = logger;
            _rateTimer = new RateTimer(options.Limit, options.Interval, _logger);
            _rateTimer.RateLimited += HandleRateLimit;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _rateTimer?.RateLimited -= HandleRateLimit;
            _rateTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
        
        private void HandleRateLimit(object? sender, RateLimitedArgs args) => LogDebug_RateLimited(_logger, args);

        /// <summary>
        /// Posts a GET request from the given <see cref="QueryBuilder"/> and <see cref="Endpoint"/>. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryBuilder">The <see cref="QueryBuilder"/> instance from which query parameters
        ///  are taken.</param>
        /// <param name="endPoint">The endpoin to query.</param>
        /// <param name="token">The cancellation token for communication cancel events.</param>
        /// <returns>A <see cref="Task"/> containing a <typeparamref name="T"/> response.</returns>
        /// <exception cref="InvalidOperationException">The response body was empty.</exception>
        private async Task<T> GetResponseAsync<T>(
            QueryBuilder queryBuilder, string endPoint, CancellationToken? token = null)
        {
            CancellationToken GetToken()
            {
                var cts = new CancellationTokenSource();
                cts.CancelAfter(_rateTimer?.ApiCallInterval ?? TimeSpan.FromSeconds(60));
                
                return cts.Token;
            }

            // Check for client-side rate limiting and await the reset if applicable.
            if(_rateTimer is not null)
            {
                var timeOut = _rateTimer.CheckLimitOrAwaitIntervalResetAsync(token);
                await timeOut;
            }

            token ??= GetToken();

            var absoluteUri = GetAbsoluteUri(endPoint);
            var uriBuilder = new UriBuilder(absoluteUri)
            {
                Query = queryBuilder.ToString()
            };
            var requestUrl = uriBuilder.Uri.AbsoluteUri;

            try
            {
                // increment counter
                _rateTimer?.Increment();

                HttpResponseMessage response = await HttpClient.GetAsync(requestUrl, cancellationToken: token.Value);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                LogDebug_ResponseHeader_Received(_logger, response.Headers);
                LogDebug_ResponseBody_Received(_logger, responseBody);

                // Parse the JSON response. If the response is null thow invalid operation
                T genericResponse = JsonConvert
                    .DeserializeObject<T>(responseBody) ??
                    throw new InvalidOperationException(message: $"{nameof(responseBody)} was null.");
                
                return genericResponse;
            }
            catch (HttpRequestException e)
            {
                LogError_HttpRequestException(_logger, e);
                throw;
            }
        }

        /// <summary>
        /// Validates the given dates form an acceptable date range parameter.
        /// </summary>
        /// <param name="dateFrom">Start date of the range tested.</param>
        /// <param name="dateTo">End date of the range tested.</param>
        /// <returns>Returns <see langword="true"/> if the range is acceptable, else throw <see cref="ArgumentException"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="dateFrom"/> is greater than <paramref name="dateTo"/> or the 
        /// range measured in days is too long.</exception>
        private bool ValidateDateRangeOrThrow(DateTime dateFrom, DateTime dateTo)
        {
            if (dateFrom > dateTo)
            {
                throw new ArgumentException(
                    $"Range invalid: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}]");
            }
            if (dateTo.Subtract(dateFrom).Days > _maximumDateRangeInDays)
            {
                throw new ArgumentException(
                    $"Range too long: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}]");
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
    }

    #region Logger methods
    public partial class MassiveApi
    {
        static void LogInfo_ResponseRequest_Submitting(ILogger? logger, object request)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger.LogInformation(eventId: 10, "Submitting request: {request}...", request);
        }

        static void LogInfo_ResponseRequest_Received(ILogger? logger, string id)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger.LogInformation(eventId: 11, "Received request response: {id}.", id);
        }

        static void LogDebug_ResponseHeader_Received(
            ILogger? logger, HttpResponseHeaders headers)
        {
            if(logger?.IsEnabled(LogLevel.Debug) ?? false)
                logger.LogDebug(eventId: 20, "Response received with headers:\n{@headers}", headers);
        }

        static void LogDebug_ResponseBody_Received(ILogger? logger, string body)
        {
            if(logger?.IsEnabled(LogLevel.Debug) ?? false)
                logger.LogDebug(eventId: 21, "Response received with body:\n{body}", body);
        }

        static void LogError_HttpRequestException(ILogger? logger, HttpRequestException exception)
        {
            if(logger?.IsEnabled(LogLevel.Error) ?? false)
                logger.LogError(eventId: 50, "HTTP request failed.\n{@exception}", exception);
        }

        static void LogDebug_RateLimited(
            ILogger? logger,
            RateLimitedArgs args)
        {
            if(logger?.IsEnabled(LogLevel.Debug) ?? false)
                    logger.LogWarning(
                        "Rate limited for {timeout}s. Estimated reset at {reset}.", 
                        args.TimeOut.TotalSeconds,
                        args.NextReset);
        }
    }
    #endregion

    #region Private, generalized methods.
    public partial class MassiveApi
    {
        /// <summary>
        /// General-purpose method for retrieving <see cref="AggregateBarResponse"/> for stocks, options, and indexes.
        /// </summary>
        /// <param name="market">Market to search.</param>
        /// <param name="ticker">The ticker of the asset. Use patterns 
        /// <list><item>O:{ticker}, for options</item>
        /// <item>I:{ticker}, for indices</item>
        /// <item>{ticker}, for stocks</item></list></param>
        /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
        /// <param name="timeSpan">Size of the time window.</param>
        /// <param name="from">Start of the time window.</param>
        /// <param name="to">End of the time window.</param>
        /// <param name="adjusted">Whether to return split-adjusted results. Default is <see langword="true"/></param>.
        /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
        /// <param name="cancellationToken">Provide a token for synchronizing cancels.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
        private async Task<AggregateBarResponse> GetGenericAggregateBarResponseAsync(
            Market market,
            string ticker,
            int multiplier,
            BarTimespan timeSpan,
            DateTime from,
            DateTime to,
            bool adjusted = true,
            int limit = 100,
            CancellationToken? cancellationToken = null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(limit, 0, nameof(limit));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 1000, nameof(limit));

            string endpointPattern = QueryBuilder
                                        .ConvertEndpointToStringPattern(Endpoint.TickerCustomBars);

            // Adjust the ticker with market-specific modifier
            string tickerAdj = market switch
            {
                Market.Crypto => $"X:{ticker}",
                Market.Fx => $"C:{ticker}",
                Market.Indices => $"I:{ticker}",
                Market.Options => $"O:{ticker}",
                Market.Stocks => ticker,
                _ => throw new InvalidOperationException($"Parameter '{nameof(market)}' must be non-default.")
            };

            string endpoint = string.Format(endpointPattern,
                                    tickerAdj,
                                    multiplier,
                                    timeSpan.ToString().ToLower(),
                                    $"{from:yyyy-MM-dd}",
                                    $"{to:yyyy-MM-dd}");

            var queryBuilder = GetQueryBuilder();
            queryBuilder.AddParameter("limit", $"{limit}");

            var response = await GetResponseAsync<AggregateBarResponse>(queryBuilder, endpoint, cancellationToken);

            return response;
        }

        /// <summary>
        /// Retrieve comprehensive details for a single ticker supported by Massive that is active as-of a given date.
        /// </summary>
        /// <param name="market">The <see cref="Market"/> to query.</param>
        /// <param name="ticker">Filter by a ticker symbol.Use patterns 
        /// <list><item>O:{ticker}, for options</item>
        /// <item>I:{ticker}, for indices</item>
        /// <item>{ticker}, for stocks</item></list></param>
        /// <param name="date">Specify a point in time to retrieve tickers available on that date. Defaults to the most recent available date.</param>
        /// <param name="cancellationToken">Provide a token for synchronizing cancels.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="TickerOverviewResponse"/>.</returns>
        private async Task<TickerOverviewResponse> GetGenericTickerOverviewResponseAsync(
            Market market,
            string ticker,
            DateTime? date = null,
            CancellationToken? cancellationToken = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(ticker);

            // Map market to endpoint since Options have a different path.
            string endpointPattern = market switch
            {
                Market.Crypto => 
                    QueryBuilder.ConvertEndpointToStringPattern(Endpoint.ReferenceTickerOverview),
                Market.Fx => 
                    QueryBuilder.ConvertEndpointToStringPattern(Endpoint.ReferenceTickerOverview),
                Market.Indices => 
                    QueryBuilder.ConvertEndpointToStringPattern(Endpoint.ReferenceTickerOverview),
                Market.Options => 
                    QueryBuilder.ConvertEndpointToStringPattern(Endpoint.ReferenceContractOverview),
                Market.Stocks => 
                    QueryBuilder.ConvertEndpointToStringPattern(Endpoint.ReferenceTickerOverview),
                _ => throw new InvalidOperationException($"Parameter '{nameof(market)}' must be non-default.")
            };

            // Adjust the ticker with market-specific modifier
            string tickerAdj = market switch
            {
                Market.Crypto => $"X:{ticker}",
                Market.Fx => $"C:{ticker}",
                Market.Indices => $"I:{ticker}",
                Market.Options => $"O:{ticker}",
                Market.Stocks => ticker,
                _ => throw new InvalidOperationException($"Parameter '{nameof(market)}' must be non-default.")
            };

            string endpoint = string.Format(endpointPattern, tickerAdj);

            var queryBuilder = GetQueryBuilder();
            if (date is not null)
                queryBuilder.AddParameter("date", $"{date:yyyy-MM-dd}");

            var response = await GetResponseAsync<TickerOverviewResponse>(queryBuilder, endpoint, cancellationToken);

            return response;
        }
    }
    #endregion
}