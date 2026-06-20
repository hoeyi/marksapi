using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ApiClient.Massive.Response;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;
using Microsoft.Extensions.Logging;

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
    public async Task<AggregateBarResponse> GetAggregateBarResponseAsync(
        Market market,
        string[] tickers,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100)
    {
        List<AggregateBarResponse> responses = [];

        if(_rateTimer is null)
                throw new InvalidOperationException(
                    $"{nameof(GetTickerOverviewResponseAsync)} requires instance of '{nameof(RateTimer)}.");

        foreach(var ticker in tickers)
        {
            await _rateTimer.CheckLimitOrAwaitIntervalResetAsync(ct: null);
            var response = await GetAggregateBarResponseAsync(market, ticker, multiplier, timeSpan, from, to, limit);
            _rateTimer.IncrementCounter();

            if(response is null)
                _logger?.LogWarning("Received empty resonse.");
            else
                responses.Add(response);
            
        }
        
        var compositeResponse = new AggregateBarResponse()
        {
            RequestId = string.Join(",", responses.Select(x => x.RequestId)),
            Ticker = string.Join(",", responses.Select(x =>x.Ticker)),
            Status = string.Join(",", responses.Select(x => x.Status)),
            Results = responses.SelectMany(x => x.Results).ToList()
        };
        return compositeResponse;
    }
}
