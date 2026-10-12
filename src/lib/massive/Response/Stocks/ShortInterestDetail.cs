using System;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks
{
    /// <summary>
    /// Represents a single short interest data point for a ticker on a specific settlement date.
    /// </summary>
    public class ShortInterestDetail
    {
        /// <summary>
        /// The average daily trading volume for the stock over a specified period,
        /// typically used to contextualize short interest.
        /// </summary>
        [JsonPropertyName("avg_daily_volume")]
        [JsonProperty(PropertyName = "avg_daily_volume")]
        public long AvgDailyVolume { get; set; }

        /// <summary>
        /// Calculated as short_interest divided by avg_daily_volume, representing the estimated
        /// number of days it would take to cover all short positions based on average trading volume.
        /// </summary>
        [JsonPropertyName("days_to_cover")]
        [JsonProperty(PropertyName = "days_to_cover")]
        public float DaysToCover { get; set; }

        /// <summary>
        /// The date (formatted as YYYY-MM-DD) on which the short interest data is considered
        /// settled, typically based on exchange reporting schedules.
        /// </summary>
        [JsonPropertyName("settlement_date")]
        [JsonProperty(PropertyName = "settlement_date")]
        public DateTime SettlementDate { get; set; }

        /// <summary>
        /// The total number of shares that have been sold short but have not yet been
        /// covered or closed out.
        /// </summary>
        [JsonPropertyName("short_interest")]
        [JsonProperty(PropertyName = "short_interest")]
        public long ShortInterest { get; set; }

        /// <summary>
        /// The primary ticker symbol for the stock.
        /// </summary>
        [JsonPropertyName("ticker")]
        [JsonProperty(PropertyName = "ticker")]
        public required string Ticker { get; set; }
    }
}