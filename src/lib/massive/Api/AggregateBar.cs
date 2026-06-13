using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;

namespace ApiClient.Massive;

public partial class MassiveApi
{
    /// <inheritdoc/>
    public async Task<AggregateBarResponse> GetAggregateBarResponseAsync(
        Market market,
        string ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100) => await GetGenericAggregateBarResponseAsync(
                                    market, ticker, multiplier, timeSpan, from, to, limit);

    /// <inheritdoc/>
    public Task<AggregateBarResponse> GetAggregateBarResponseAsync(
        Market market,
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
