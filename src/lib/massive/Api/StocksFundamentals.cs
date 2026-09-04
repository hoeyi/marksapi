using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive.Parameters;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;
using Microsoft.Extensions.Logging;

namespace ApiClient.Massive
{
    public partial class MassiveApi
    {
        /// <inheritdoc/>
        public async Task<ShortInterestResponse> GetShortInterestResponseAsync(
            string[]? tickers = null,
            DateTime? settlementDate = null,
            Dictionary<NumericComparisonOperator, float>? daysToCover = null,
            Dictionary<NumericComparisonOperator, float>? averageDailyVolume = null,
            int? limit = 10,
            CancellationToken? cancellationToken = null)
        {
            // Entries will be dropped, but log a warning if possible.
            string[] nonEmptyTickers = tickers?.Where(x => string.IsNullOrEmpty(x)).ToArray() ?? [];
            if(nonEmptyTickers.Length == 0)
                _logger?.LogWarning("Ignoring empty entries in '{parameter}'.", nameof(tickers));

            var queryBuilder = GetQueryBuilder();
            if(nonEmptyTickers.Length > 0)
            {
                var tickersDelimited = string.Join(",", nonEmptyTickers);
                queryBuilder.AddParameter("ticker.any_of", string.Join(",", tickersDelimited));
            }

            if(daysToCover?.Count > 0)
            {
                foreach(var kv in daysToCover)
                queryBuilder.AddParameter(
                    $"days_to_cover.{kv.Key.ToString().ToLower()}", $"{kv.Value}");
            }

            if(averageDailyVolume?.Count > 0)
            {
                foreach(var kv in averageDailyVolume)
                queryBuilder.AddParameter(
                    $"avg_daily_volume.{kv.Key.ToString().ToLower()}", $"{kv.Value}");
            }

            queryBuilder.AddParameter("limit", $"{limit}");

            var response = await GetResponseAsync<ShortInterestResponse>(
                                    queryBuilder, 
                                    Endpoint.StocksFundamentalsShortInterest,
                                    cancellationToken);

            return response;
        }

        /// <inheritdoc/>
        public async Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
            string[]? tickers,
            DateTime fromDate,
            DateTime toDate,
            Interval<float>? shortVolumeRatio = null,
            int? limit = 10,
            CancellationToken? cancellationToken = null)
        {
            // Entries will be dropped, but log a warning if possible.
            string[] nonEmptyTickers = tickers?.Where(x => string.IsNullOrEmpty(x)).ToArray() ?? [];
            if(nonEmptyTickers.Length == 0)
                _logger?.LogWarning("Ignoring empty entries in '{parameter}'.", nameof(tickers));

            ArgumentOutOfRangeException.ThrowIfGreaterThan(fromDate, toDate);

            var queryBuilder = GetQueryBuilder();

            if(nonEmptyTickers.Length > 0)
            {
                var tickersDelimited = string.Join(",", nonEmptyTickers);
                queryBuilder.AddParameter("ticker.any_of", string.Join(",", tickersDelimited));
            }

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
}

