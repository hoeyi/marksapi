using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;

namespace ApiClient.Massive;

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
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
    public async Task<AggregateBarResponse> GetOptionsAggregateBarResponseAsync(
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
                                    .ConvertEndpointToStringPattern(Endpoint.OptionsCustomBars);

        string endpoint = string.Format(endpointPattern, 
                                $"O:{ticker}", 
                                multiplier, 
                                timeSpan.ToString().ToLower(), 
                                $"{from:yyyy-MM-dd}", 
                                $"{to:yyyy-MM-dd}");

        var queryBuilder = GetQueryBuilder();
        queryBuilder.AddParameter("limit", $"{limit}");
        
        var response = await GetResponseAsync<AggregateBarResponse>(queryBuilder, endpoint);
        
        return response;
    }
}
