using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        BarTimespan timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100,
        CancellationToken? cancellationToken = null)
    {
        var guid = Guid.NewGuid();
        LogInfo_ResponseRequest_Submitting(_logger, new
        {
            id = guid.ToString("N"),
            market,
            ticker,
            from,
            to,
            multiplier,
            timeSpan
        });
        var result = await GetGenericAggregateBarResponseAsync(
            market,
            ticker,
            multiplier,
            timeSpan,
            from,
            to,
            limit,
            cancellationToken);

        LogInfo_ResponseRequest_Received(_logger, guid.ToString("N"));

        return result;
    } 

    /// <inheritdoc/>
    public async Task<List<AggregateBarResponse>> GetAggregateBarResponseAsync(
        Market market,
        string[] tickers,
        int multiplier,
        BarTimespan timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100,
        CancellationToken? cancellationToken = null)
    {
        List<AggregateBarResponse> responses = [];

        if(_rateTimer is null)
                throw new InvalidOperationException(
                    $"{nameof(GetTickerOverviewResponseAsync)} requires instance of '{nameof(RateTimer)}.");

        foreach(var ticker in tickers)
        {
            await _rateTimer.CheckLimitOrAwaitIntervalResetAsync(ct: cancellationToken);
            var response = await GetAggregateBarResponseAsync(
                            market,
                            ticker,
                            multiplier,
                            timeSpan,
                            from,
                            to,
                            limit,
                            cancellationToken);
            _rateTimer.Increment();

            if(response is null)
                _logger?.LogWarning("Received empty resonse.");
            else
                responses.Add(response);
            
        }
        
        return responses;
    }
}
