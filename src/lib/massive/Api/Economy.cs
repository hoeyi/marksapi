using ApiClient.Massive.Parameters;
using ApiClient.Massive.Response.Economy;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiClient.Massive
{
    public partial class MassiveApi : IMassiveApi
    {
        
        /// <inheritdoc/>
        public async Task<InflationResponse> GetInflationResponseAsync(
            Dictionary<NumericComparisonOperator, DateTime>? dateFilter = null,
            int? limit = 100,
            CancellationToken? cancellationToken = null)
        {
            if(_rateTimer is null)
                    throw new InvalidOperationException(
                        $"{nameof(GetInflationResponseAsync)} requires instance of '{nameof(RateTimer)}.");
            await _rateTimer.CheckLimitOrAwaitIntervalResetAsync(cancellationToken);
        
            var queryBuilder = GetQueryBuilder();

            queryBuilder.AddComparisonFilterParameters(
                            "date",
                            dateFilter,
                            customFormat: QueryBuilderExtensions.DateFormat);
            queryBuilder.AddParameter("limit", $"{limit}");

            var response = await GetResponseAsync<InflationResponse>(
                queryBuilder,
                Endpoint.Inflation,
                cancellationToken);

            return response;
        }

        /// <inheritdoc/>
        public async Task<InflationExpectationResponse> GetInflationExpectationResponseAsync(
            Dictionary<NumericComparisonOperator, DateTime>? dateFilter = null,
            int? limit = 100,
            CancellationToken? cancellationToken = null)
        {
            if(_rateTimer is null)
                    throw new InvalidOperationException(
                        $"{nameof(GetInflationExpectationResponseAsync)} requires instance of '{nameof(RateTimer)}.");
            await _rateTimer.CheckLimitOrAwaitIntervalResetAsync(cancellationToken);
        
            var queryBuilder = GetQueryBuilder();

            queryBuilder.AddComparisonFilterParameters(
                            "date",
                            dateFilter,
                            customFormat: QueryBuilderExtensions.DateFormat);

            queryBuilder.AddParameter("limit", $"{limit}");

            var response = await GetResponseAsync<InflationExpectationResponse>(
                queryBuilder,
                Endpoint.InflationExpectations,
                cancellationToken);

            return response;
        }

        /// <inheritdoc/>
        public async Task<LaborMarketResponse> GetLaborMarketResponseAsync(
            Dictionary<NumericComparisonOperator, DateTime>? dateFilter = null,
            int? limit = 100,
            CancellationToken? cancellationToken = null)
        {
            if(_rateTimer is null)
                    throw new InvalidOperationException(
                        $"{nameof(GetLaborMarketResponseAsync)} requires instance of '{nameof(RateTimer)}.");
            await _rateTimer.CheckLimitOrAwaitIntervalResetAsync(cancellationToken);
        

            var queryBuilder = GetQueryBuilder();

            queryBuilder.AddComparisonFilterParameters(
                            "date",
                            dateFilter,
                            customFormat: QueryBuilderExtensions.DateFormat);

            queryBuilder.AddParameter("limit", $"{limit}");

            var response = await GetResponseAsync<LaborMarketResponse>(
                queryBuilder,
                Endpoint.LaborMarket,
                cancellationToken);

            return response;
        }

        /// <inheritdoc/>
        public async Task<TreasuryYieldsResponse> GetTreasuryYieldResponseAsync(
            Dictionary<NumericComparisonOperator, DateTime>? dateFilter = null,
            int? limit = 100,
            CancellationToken? cancellationToken = null)
        {
            if(_rateTimer is null)
                    throw new InvalidOperationException(
                        $"{nameof(GetTreasuryYieldResponseAsync)} requires instance of '{nameof(RateTimer)}.");
            await _rateTimer.CheckLimitOrAwaitIntervalResetAsync(cancellationToken);

            var queryBuilder = GetQueryBuilder();

            queryBuilder.AddComparisonFilterParameters(
                            "date",
                            dateFilter,
                            customFormat: QueryBuilderExtensions.DateFormat);
            queryBuilder.AddParameter("limit", $"{limit}");

            var response = await GetResponseAsync<TreasuryYieldsResponse>(
                queryBuilder,
                Endpoint.TreasuryYields,
                cancellationToken);

            return response;
        }
    }
}

