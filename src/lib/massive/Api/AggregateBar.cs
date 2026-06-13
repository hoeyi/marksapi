using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;

namespace ApiClient.Massive;

public partial class MassiveApi
{
    /// <inheritdoc/>
    public async Task<AggregateBarResponse> GetIndexAggregateBarResponseAsync(
        string ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100) => await GetGenericAggregateBarResponseAsync(
                                    $"I:{ticker}", multiplier, timeSpan, from, to, limit);

    /// <inheritdoc/>
    public async Task<AggregateBarResponse> GetIndexAggregateBarResponseAsync(
        string[] ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public async Task<AggregateBarResponse> GetOptionsAggregateBarResponseAsync(
        string ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100) => await GetGenericAggregateBarResponseAsync(
                                    $"O:{ticker}", multiplier, timeSpan, from, to, limit);

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

    /// <inheritdoc/>
    public async Task<AggregateBarResponse> GetStocksAggregateBarResponseAsync(
        string ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100) => await GetGenericAggregateBarResponseAsync(
                                    ticker, multiplier, timeSpan, from, to, limit);

    /// <inheritdoc/>
    public Task<AggregateBarResponse> GetStocksAggregateBarResponseAsync(
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
