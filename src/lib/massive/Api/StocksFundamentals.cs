using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;

namespace ApiClient.Massive;

public partial class MassiveApi
{
    /// <summary>
    /// Retrieve daily aggregated short sale volume data reported to FINRA from off-exchange trading 
    /// venues and alternative trading systems (ATS) for a specified stock ticker.
    /// </summary>
    /// <param name="ticker">The primary ticker symbol for the stock.</param>
    /// <param name="fromDate">The start date of trade activity.</param>
    /// <param name="toDate">The end date of trade activity.</param>
    /// <param name="shortVolumeRatio">Interval for filtering results.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 50000, Default = 10).</param>
    /// <returns></returns>
    public async Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
        string ticker,
        DateTime fromDate,
        DateTime toDate,
        Interval<float> shortVolumeRatio,
        int? limit = 10
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(ticker);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(fromDate, toDate);

        var queryBuilder = GetQueryBuilder();
        queryBuilder.AddParameter("date.gte", $"{fromDate:yyyy-MM-dd}");
        queryBuilder.AddParameter("date.lte", $"{toDate:yyyy-MM-dd}");
        queryBuilder.AddParameter(
            shortVolumeRatio.OpenLeft ? "short_volume_ratio.gt" : "short_volume_ratio.gte",
            $"{shortVolumeRatio.Start}");
        queryBuilder.AddParameter(
            shortVolumeRatio.OpenRight ? "short_volume_ratio.lt" : "short_volume_ratio.lte",
            $"{shortVolumeRatio.End}");
        queryBuilder.AddParameter("limit", $"{limit}");
        
        var response = await GetResponseAsync<ShortVolumeResponse>(
                                queryBuilder, Endpoint.StocksFundamentalsShortVolume);
        
        return response;
    }   
}
