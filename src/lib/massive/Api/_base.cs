using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ApiClient.Services;
using ApiClient.Resources;

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

        public MassiveApi(string apiKey, ILogger? logger = null)
            : this(new HttpClient(), apiKey, logger)
        {
        }

        internal MassiveApi(
            HttpClient httpClient,
            string apiKey,
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
                _logger?.LogDebug("{responseBody}", responseBody);
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
            public const string OptionsCustomBars = "/v2/aggs/ticker/{optionsTicker}/range/{multiplier}/{timespan}/{from}/{to}";

            // TODO: Impement endpoint
            // public const string OptionsTickerSummary = "/v1/open-close/{optionsTicker}/{date}";

            // TODO: Impement endpoint
            // public const string OptionsPreviousDayBar = "/v2/aggs/ticker/{optionsTicker}/prev";

            public const string StocksAllTickers = "/v3/reference/tickers";

            public const string StocksTickerOverview = "/v3/reference/tickers/{ticker}";

            public const string StocksTickerTypes = "/v3/reference/tickers/types";

            // TODO: Impement endpoint
            // public const string StocksRelatedTickers = "/v1/related-companies/{ticker}";
            public const string StocksCustomBars = "/v2/aggs/ticker/{stocksTicker}/range/{multiplier}/{timespan}/{from}/{to}";

            // TODO: Impement endpoint
            // public const string StocksDailySummary = "/v2/aggs/grouped/locale/us/market/stocks/{date}";

            // TODO: Impement endpoint
            // public const string StocksTickerSummary = "/v1/open-close/{stocksTicker}/{date}";

            // TODO: Impement endpoint
            // public const string StocksPreviousDayBar = "/v2/aggs/ticker/{stocksTicker}/prev";

            public const string StocksFundamentalsShortVolume = "/stocks/v1/short-volume";
        }
    }
}