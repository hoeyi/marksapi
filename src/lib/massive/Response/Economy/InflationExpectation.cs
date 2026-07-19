using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Economy;

/// <summary>
/// Represents a single observation of inflation expectations for a given date.
/// Captures market and model-based forecasts across different time horizons.
/// </summary>
public class InflationExpectation
{
    /// <summary>
    /// Calendar date of the observation in YYYY-MM-DD format.
    /// </summary>
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    /// <summary>
    /// 5-Year, 5-Year Forward Inflation Expectation Rate — the market's expectation of average annual inflation for the 5-year period beginning 5 years from now, based on the spread between forward nominal and real yields.
    /// </summary>
    [JsonPropertyName("forward_years_5_to_10")]
    public double? ForwardYears5To10 { get; set; }

    /// <summary>
    /// 10-Year Breakeven Inflation Rate — the market's expectation of average annual inflation over the next 10 years, based on the spread between 10-year nominal Treasury yields and 10-year TIPS yields.
    /// </summary>
    [JsonPropertyName("market_10_year")]
    public double? Market10Year { get; set; }

    /// <summary>
    /// 5-Year Breakeven Inflation Rate — the market's expectation of average annual inflation over the next 5 years, based on the spread between 5-year nominal Treasury yields and 5-year TIPS yields.
    /// </summary>
    [JsonPropertyName("market_5_year")]
    public double? Market5Year { get; set; }

    /// <summary>
    /// The Cleveland Fed's 10-year inflation expectations data estimating expected inflation, risk premiums, and the real interest rate using a model based on Treasury yields, inflation data, swaps, and surveys.
    /// </summary>
    [JsonPropertyName("model_10_year")]
    public double? Model10Year { get; set; }

    /// <summary>
    /// The Cleveland Fed's 1-year inflation expectations data estimating expected inflation, risk premiums, and the real interest rate using a model based on Treasury yields, inflation data, swaps, and surveys.
    /// </summary>
    [JsonPropertyName("model_1_year")]
    public double? Model1Year { get; set; }

    /// <summary>
    /// The Cleveland Fed's 30-year inflation expectations data estimating expected inflation, risk premiums, and the real interest rate using a model based on Treasury yields, inflation data, swaps, and surveys.
    /// </summary>
    [JsonPropertyName("model_30_year")]
    public double? Model30Year { get; set; }

    /// <summary>
    /// The Cleveland Fed's 5-year inflation expectations data estimating expected inflation, risk premiums, and the real interest rate using a model based on Treasury yields, inflation data, swaps, and surveys.
    /// </summary>
    [JsonPropertyName("model_5_year")]
    public double? Model5Year { get; set; }
}