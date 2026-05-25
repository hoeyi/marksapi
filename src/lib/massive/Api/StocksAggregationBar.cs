using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApiClient.Massive.Response;
using ApiClient.Services;

namespace ApiClient.Massive
{
    /// <inheritdoc/>
    public partial class MassiveApi
    {
        /// <summary>
        /// Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for a 
        /// specified stock ticker over a custom date range and time interval in Eastern Time (ET).
        /// </summary>
        /// <param name="ticker">Case-sensitive ticker symbol.</param>
        /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
        /// <param name="timeSpan">Size of the time window.</param>
        /// <param name="from">Start of the time window.</param>
        /// <param name="to">End of the time window.</param>
        /// <param name="limit">Maximum number of records to return. Maximum value is 1000.</param>
        /// <returns></returns>
        public async Task<AggregateBarResponse> GetAggregateBarResponseAsync(
            string ticker, int multiplier, BarTimespan timeSpan, DateTime from, DateTime to, int limit = 100)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(limit, 0, nameof(limit));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 1000, nameof(limit));
            
            string endpointPattern = QueryBuilder
                                        .ConvertEndpointToStringPattern(Endpoint.StocksCustomBars);

            string endpoint = string.Format(endpointPattern, 
                                    ticker, 
                                    multiplier, 
                                    timeSpan, 
                                    $"{from:yyyy-MM-dd}", 
                                    $"{from:yyyy-MM-dd}");

            var queryBuilder = GetQueryBuilder();
            var response = await GetResponseAsync<AggregateBarResponse>(queryBuilder, endpoint);
            
            return response;
        }
    }
}
