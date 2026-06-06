using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;

namespace ApiClient.Massive;

public partial class MassiveApi
{
    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public Task<AggregateBarResponse> GetOptionsAggregateBarResponseAsync(
        string[] ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100)
    {
        throw new NotImplementedException();
    }    
}
