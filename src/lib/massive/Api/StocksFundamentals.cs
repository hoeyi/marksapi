using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;

namespace ApiClient.Massive;

public partial class MassiveApi
{
    /// <inheritdoc/>
    public async Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
        string ticker,
        DateTime fromDate,
        DateTime toDate,
        Interval<float>? shortVolumeRatio = null,
        int? limit = 10
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(ticker);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(fromDate, toDate);

        var queryBuilder = GetQueryBuilder();
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
                                queryBuilder, Endpoint.StocksFundamentalsShortVolume);

        return response;
    }

    /// <inheritdoc/>
    public Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
        string[] ticker,
        DateTime fromDate,
        DateTime toDate,
        Interval<float>? shortVolumeRatio = null,
        int? limit = 10)
    {
        throw new NotImplementedException();
    }
}
