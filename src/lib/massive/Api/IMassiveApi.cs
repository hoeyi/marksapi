using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;

namespace ApiClient.Massive;

/// <summary>
/// Represents an interface for interacting with the <strong>Massive API</strong>, <a href="https://massive.com"/>.
/// </summary>
public interface IMassiveApi
{
    /// <summary>
    /// Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for a 
    /// specified option ticker over a custom date range and time interval in Eastern Time (ET).
    /// </summary>
    /// <param name="ticker">Case-sensitive ticker symbol(s).</param>
    /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
    /// <param name="timeSpan">Size of the time window.</param>
    /// <param name="from">Start of the time window.</param>
    /// <param name="to">End of the time window.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
    Task<AggregateBarResponse> GetOptionsAggregateBarResponseAsync(
        string[] ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100);

    /// <summary>
    /// Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for a 
    /// specified stock ticker over a custom date range and time interval in Eastern Time (ET).
    /// </summary>
    /// <param name="ticker">Case-sensitive ticker symbol.</param>
    /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
    /// <param name="timeSpan">Size of the time window.</param>
    /// <param name="from">Start of the time window.</param>
    /// <param name="to">End of the time window.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
    Task<AggregateBarResponse> GetOptionsAggregateBarResponseAsync(
        string ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100);

    /// <summary>
    /// Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for a 
    /// specified index ticker over a custom date range and time interval in Eastern Time (ET).
    /// </summary>
    /// <param name="ticker">Case-sensitive ticker symbol(s).</param>
    /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
    /// <param name="timeSpan">Size of the time window.</param>
    /// <param name="from">Start of the time window.</param>
    /// <param name="to">End of the time window.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
    Task<AggregateBarResponse> GetIndexAggregateBarResponseAsync(
        string ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100);
    
    /// <summary>
    /// Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for a 
    /// specified index tickers over a custom date range and time interval in Eastern Time (ET).
    /// </summary>
    /// <param name="ticker">Case-sensitive ticker symbol(s).</param>
    /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
    /// <param name="timeSpan">Size of the time window.</param>
    /// <param name="from">Start of the time window.</param>
    /// <param name="to">End of the time window.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
    Task<AggregateBarResponse> GetIndexAggregateBarResponseAsync(
        string[] ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100);

    /// <summary>
    /// Retrieve daily aggregated short sale volume data reported to FINRA from off-exchange trading 
    /// venues and alternative trading systems (ATS) for a specified stock ticker.
    /// </summary>
    /// <param name="ticker">The primary ticker symbol for the stock(s).</param>
    /// <param name="fromDate">The start date of trade activity.</param>
    /// <param name="toDate">The end date of trade activity.</param>
    /// <param name="shortVolumeRatio">Interval for filtering results.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 50000, Default = 10).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="ShortVolumeResponse"/>.</returns>
    Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
        string[] ticker,
        DateTime fromDate,
        DateTime toDate,
        Interval<float>? shortVolumeRatio = null,
        int? limit = 10);

    /// <summary>
    /// Retrieve daily aggregated short sale volume data reported to FINRA from off-exchange trading 
    /// venues and alternative trading systems (ATS) for a specified stock ticker.
    /// </summary>
    /// <param name="ticker">The primary ticker symbol for the stock.</param>
    /// <param name="fromDate">The start date of trade activity.</param>
    /// <param name="toDate">The end date of trade activity.</param>
    /// <param name="shortVolumeRatio">Interval for filtering results.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 50000, Default = 10).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="ShortVolumeResponse"/>.</returns>
    Task<ShortVolumeResponse> GetShortVolumeResponseAsync(
        string ticker,
        DateTime fromDate,
        DateTime toDate,
        Interval<float>? shortVolumeRatio = null,
        int? limit = 10);

    /// <summary>
    /// Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for a 
    /// specified stock ticker over a custom date range and time interval in Eastern Time (ET).
    /// </summary>
    /// <param name="ticker">Case-sensitive ticker symbol(s).</param>
    /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
    /// <param name="timeSpan">Size of the time window.</param>
    /// <param name="from">Start of the time window.</param>
    /// <param name="to">End of the time window.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
    Task<AggregateBarResponse> GetStocksAggregateBarResponseAsync(
        string[] ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100);

    /// <summary>
    /// Retrieve aggregated historical OHLC (Open, High, Low, Close) and volume data for a 
    /// specified stock ticker over a custom date range and time interval in Eastern Time (ET).
    /// </summary>
    /// <param name="ticker">Case-sensitive ticker symbol.</param>
    /// <param name="multiplier">Timespan multiplier, e.g., 1 {timeSpan}.</param>
    /// <param name="timeSpan">Size of the time window.</param>
    /// <param name="from">Start of the time window.</param>
    /// <param name="to">End of the time window.</param>
    /// <param name="limit">Maximum number of records to return (Min = 1, Max = 1000, Default = 100).</param>
    /// <returns>A <see cref="Task"/> containing an <see cref="AggregateBarResponse"/>.</returns>
    Task<AggregateBarResponse> GetStocksAggregateBarResponseAsync(
        string ticker,
        int multiplier,
        BarTimespanEnum timeSpan,
        DateTime from,
        DateTime to,
        int limit = 100);

    /// <summary>
    /// Submits queries to the endpoint <em>/v3/reference/tickers</em>.
    /// </summary>
    /// <param name="ticker">Filter by a ticker symbol. Defaults to empty string which queries all tickers.</param>
    /// <param name="type">Filter by the type of the tickers. Defaults to empty string which queries all types.</param>
    /// <param name="market">Filter by market type. By default all markets are included.</param>
    /// <param name="exchange">Filter by the asset's primary exchange Market Identifier Code (MIC) according to ISO 10383. Defaults to empty string which queries all exchanges.</param>
    /// <param name="cusip">Filter by the CUSIP code of the asset you want to search for.</param>
    /// <param name="cik">Filter by the Central Index Key of the asset.</param>
    /// <param name="date">Specify a point in time to retrieve tickers available on that date. Defaults to the most recent available date.</param>
    /// <param name="search">Filter for terms within the ticker and/or company name.</param>
    /// <param name="active">Filter for active tickers only.</param>
    /// <param name="asc">Sort the results by ascending order.</param>
    /// <param name="sort">The field to sort by.</param>
    /// <param name="limit">Limit the number of results returned, default is 100 and max is 1000.</param>
    /// <returns>A <see cref="Task"/> containing a <see cref="AggregateTickerResponse"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="limit"/> was not in the interval (0,1000].</exception>
    Task<AggregateTickerResponse> GetAllTickersAsync(
        string? ticker = null,
        TickerType? type = null,
        string? market = null,
        string? exchange = null,
        string? cusip = null,
        string? cik = null,
        DateTime? date = null,
        string? search = null,
        bool active = true,
        bool asc = true,
        string? sort = null,
        int limit = 100);

    /// <summary>
    /// Retrieve comprehensive details for a single ticker supported by Massive that is active as-of a given date.
    /// </summary>
    /// <param name="ticker">Filter by a ticker symbol(s).</param>
    /// <param name="date">Specify a point in time to retrieve tickers available on that date. Defaults to the most recent available date.</param>
    /// <returns>A <see cref="Task"/> containing a <see cref="TickerOverviewResponse"/>.</returns>
    Task<TickerOverviewResponse> GetAllTickerOverviewResponseAsync(
        string[] ticker,
        DateTime? date = null);

    /// <summary>
    /// Retrieve comprehensive details for a single ticker supported by Massive that is active as-of a given date.
    /// </summary>
    /// <param name="ticker">Filter by a ticker symbol.</param>
    /// <param name="date">Specify a point in time to retrieve tickers available on that date. Defaults to the most recent available date.</param>
    /// <returns>A <see cref="Task"/> containing a <see cref="TickerOverviewResponse"/>.</returns>
    Task<TickerOverviewResponse> GetTickerOverviewResponseAsync(
        string ticker,
        DateTime? date = null);
}
