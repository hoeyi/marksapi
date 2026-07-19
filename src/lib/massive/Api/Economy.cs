using ApiClient.Massive.Parameters;
using ApiClient.Massive.Response;
using ApiClient.Massive.Response.Economy;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiClient.Massive;

public partial class MassiveApi : IMassiveApi
{
    /// <inheritdoc/>
    public async Task<TreasuryYieldsResponse> GetTreasuryYieldResponseAsync(
        DateTime[] dates,
        NumericComparisonOperator? numOp = null,
        int? limit = 100,
        CancellationToken? cancellationToken = null)
    {
        if(numOp is not null && dates.Length > 1)
            throw new ArgumentException(
                $"Parameter '{nameof(dates)}' expects length 1 if '{nameof(numOp)}' provided.");

        var queryBuilder = GetQueryBuilder();

        queryBuilder.AddParameter(
            "date.any_of", string.Join(",", dates.Select(x => x.ToString("YYYY-MM-DD"))));
        queryBuilder.AddParameter("limit", $"{limit}");

        var response = await GetResponseAsync<TreasuryYieldsResponse>(
            queryBuilder,
            Endpoint.TreasuryYields,
            cancellationToken);

        return response;
    }

    /// <inheritdoc/>
    public async Task<InflationResponse> GetInflationResponseAsync(
        DateTime[] dates,
        NumericComparisonOperator? numOp = null,
        int? limit = 100,
        CancellationToken? cancellationToken = null)
    {
        if(numOp is not null && dates.Length > 1)
            throw new ArgumentException(
                $"Parameter '{nameof(dates)}' expects length 1 if '{nameof(numOp)}' provided.");

        var queryBuilder = GetQueryBuilder();

        queryBuilder.AddParameter(
            "date.any_of", string.Join(",", dates.Select(x => x.ToString("YYYY-MM-DD"))));
        queryBuilder.AddParameter("limit", $"{limit}");

        var response = await GetResponseAsync<InflationResponse>(
            queryBuilder,
            Endpoint.Inflation,
            cancellationToken);

        return response;
    }

    /// <inheritdoc/>
    public async Task<InflationExpectationResponse> GetInflationExpectationResponseAsync(
        DateTime[] dates,
        NumericComparisonOperator? numOp = null,
        int? limit = 100,
        CancellationToken? cancellationToken = null)
    {
        if(numOp is not null && dates.Length > 1)
            throw new ArgumentException(
                $"Parameter '{nameof(dates)}' expects length 1 if '{nameof(numOp)}' provided.");

        var queryBuilder = GetQueryBuilder();

        queryBuilder.AddParameter(
            "date.any_of", string.Join(",", dates.Select(x => x.ToString("YYYY-MM-DD"))));
        queryBuilder.AddParameter("limit", $"{limit}");

        var response = await GetResponseAsync<InflationExpectationResponse>(
            queryBuilder,
            Endpoint.InflationExpectations,
            cancellationToken);

        return response;
    }

    /// <inheritdoc/>
    public async Task<LaborMarketResponse> GetLaborMarketResponseAsync(
        DateTime[] dates,
        NumericComparisonOperator? numOp = null,
        int? limit = 100,
        CancellationToken? cancellationToken = null)
    {
        if(numOp is not null && dates.Length > 1)
            throw new ArgumentException(
                $"Parameter '{nameof(dates)}' expects length 1 if '{nameof(numOp)}' provided.");

        var queryBuilder = GetQueryBuilder();

        queryBuilder.AddParameter(
            "date.any_of", string.Join(",", dates.Select(x => x.ToString("YYYY-MM-DD"))));
        queryBuilder.AddParameter("limit", $"{limit}");

        var response = await GetResponseAsync<LaborMarketResponse>(
            queryBuilder,
            Endpoint.LaborMarket,
            cancellationToken);

        return response;
    }    
}
