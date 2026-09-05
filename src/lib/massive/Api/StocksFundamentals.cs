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

            queryBuilder.AddAnyParameter("ticker", nonEmptyTickers);
            queryBuilder.AddComparisonFilterParameters("days_to_cover", daysToCover);
            queryBuilder.AddComparisonFilterParameters("avg_daily_volume", averageDailyVolume);

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
            Dictionary<NumericComparisonOperator, DateTime>? dateFilter = null,
            Dictionary<NumericComparisonOperator, float>? shortVolumeRatio = null,
            int? limit = 10,
            CancellationToken? cancellationToken = null)
        {
            // Entries will be dropped, but log a warning if possible.
            string[] nonEmptyTickers = tickers?.Where(x => string.IsNullOrEmpty(x)).ToArray() ?? [];
            if(nonEmptyTickers.Length == 0)
                _logger?.LogWarning("Ignoring empty entries in '{parameter}'.", nameof(tickers));

            var queryBuilder = GetQueryBuilder();

            queryBuilder.AddAnyParameter("ticker", nonEmptyTickers);
            queryBuilder.AddComparisonFilterParameters(
                            "date",
                            dateFilter,
                            customFormat: QueryBuilderExtensions.DateFormat);
            queryBuilder.AddComparisonFilterParameters("short_volume_ratio", shortVolumeRatio);

            queryBuilder.AddParameter("limit", $"{limit}");

            var response = await GetResponseAsync<ShortVolumeResponse>(
                                    queryBuilder, 
                                    Endpoint.StocksFundamentalsShortVolume,
                                    cancellationToken);

            return response;
        }
    }
}

