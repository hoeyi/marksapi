using Newtonsoft.Json;

namespace MarketStackApi
{
    /// <summary>
    /// Represents the response data from the <b><em>/bond</em></b> endpoint.
    /// </summary>
    public class BondInfo
    {
        /// <summary>
        /// Region where the bond is supported.
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Country where the bond is supported.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Bond tenor/type (for example 10Y).
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Current bond yield.
        /// </summary>
        [JsonProperty("yield")]
        public string Yield { get; set; } = string.Empty;

        /// <summary>
        /// Price change day-over-day.
        /// </summary>
        [JsonProperty("price_change_day")]
        public string PriceChangeDay { get; set; } = string.Empty;

        /// <summary>
        /// Weekly change percentage.
        /// </summary>
        [JsonProperty("percentage_week")]
        public string PercentageWeek { get; set; } = string.Empty;

        /// <summary>
        /// Monthly change percentage.
        /// </summary>
        [JsonProperty("percentage_month")]
        public string PercentageMonth { get; set; } = string.Empty;

        /// <summary>
        /// Yearly change percentage.
        /// </summary>
        [JsonProperty("percentage_year")]
        public string PercentageYear { get; set; } = string.Empty;

        /// <summary>
        /// Quote date.
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; } = string.Empty;
    }
}
