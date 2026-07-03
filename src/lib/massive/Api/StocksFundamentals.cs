using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;
using Microsoft.Extensions.Logging;

namespace ApiClient.Massive;

public partial class MassiveApi
{
    /// <inheritdoc/>
    public async Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
        string ticker,
        DateTime fromDate,
        DateTime toDate,
        Interval<float>? shortVolumeRatio = null,
        int? limit = 10,
        CancellationToken? cancellationToken = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(ticker);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(fromDate, toDate);

        var queryBuilder = GetQueryBuilder();
        queryBuilder.AddParameter("ticker", ticker);
        queryBuilder.AddParameter("date.gte", $"{fromDate:yyyy-MM-dd}");
        queryBuilder.AddParameter("date.lte", $"{toDate:yyyy-MM-dd}");

        if (shortVolumeRatio.HasValue)
        {
            queryBuilder.AddParameter(
                shortVolumeRatio.Value.OpenLeft ? "short_volume_ratio.gt" : "short_volume_ratio.gte",
                $"{shortVolumeRatio.Value.Start}");
            queryBuilder.AddParameter(
                shortVolumeRatio.Value.OpenRight ? "short_volume_ratio.lt" : "short_volume_ratio.lte",
                $"{shortVolumeRatio.Value.End}");
        }

        queryBuilder.AddParameter("limit", $"{limit}");

        var response = await GetResponseAsync<ShortVolumeResponse>(
                                queryBuilder, 
                                Endpoint.StocksFundamentalsShortVolume,
                                cancellationToken);

        return response;
    }

    /// <inheritdoc/>
    public async Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
        string[] tickers,
        DateTime fromDate,
        DateTime toDate,
        Interval<float>? shortVolumeRatio = null,
        int? limit = 10,
        CancellationToken? cancellationToken = null)
    {
        if(tickers.Length == 0)
            throw new ArgumentException($"Parameter '{tickers} must be non-empty.");
        
        // Entries will be dropped, but log a warning if possible.
        if(tickers.Any(x => string.IsNullOrEmpty(x)))
            _logger?.LogWarning("Ignoring empty entries in '{parameter}'.", nameof(tickers));

        ArgumentOutOfRangeException.ThrowIfGreaterThan(fromDate, toDate);

        var queryBuilder = GetQueryBuilder();
        var tickersDelimited = string.Join(",", tickers.Where(x => !string.IsNullOrEmpty(x)));
        queryBuilder.AddParameter("ticker.any_of", string.Join(",", tickersDelimited));
        queryBuilder.AddParameter("date.gte", $"{fromDate:yyyy-MM-dd}");
        queryBuilder.AddParameter("date.lte", $"{toDate:yyyy-MM-dd}");

        if (shortVolumeRatio.HasValue)
        {
            queryBuilder.AddParameter(
                shortVolumeRatio.Value.OpenLeft ? "short_volume_ratio.gt" : "short_volume_ratio.gte",
                $"{shortVolumeRatio.Value.Start}");
            queryBuilder.AddParameter(
                shortVolumeRatio.Value.OpenRight ? "short_volume_ratio.lt" : "short_volume_ratio.lte",
                $"{shortVolumeRatio.Value.End}");
        }

        queryBuilder.AddParameter("limit", $"{limit}");

        var response = await GetResponseAsync<ShortVolumeResponse>(
                                queryBuilder, 
                                Endpoint.StocksFundamentalsShortVolume,
                                cancellationToken);

        return response;
    }
}
