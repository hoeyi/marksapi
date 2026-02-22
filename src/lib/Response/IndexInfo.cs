using Newtonsoft.Json;

namespace MarketStackApi
{
    /// <summary>
    /// Represents the response data from the <b><em>/indexinfo</em></b> endpoint.
    /// </summary>
    public class IndexInfo
    {
        /// <summary>
        /// Benchmark name of the market index.
        /// </summary>
        [JsonProperty("benchmark")]
        public string Benchmark { get; set; } = string.Empty;

        /// <summary>
        /// Region of the index.
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Country of the index.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Current index price.
        /// </summary>
        [JsonProperty("price")]
        public string Price { get; set; } = string.Empty;

        /// <summary>
        /// Absolute day change.
        /// </summary>
        [JsonProperty("price_change_day")]
        public string PriceChangeDay { get; set; } = string.Empty;

        /// <summary>
        /// Day change in percent.
        /// </summary>
        [JsonProperty("percentage_day")]
        public string PercentageDay { get; set; } = string.Empty;

        /// <summary>
        /// Week change in percent.
        /// </summary>
        [JsonProperty("percentage_week")]
        public string PercentageWeek { get; set; } = string.Empty;

        /// <summary>
        /// Month change in percent.
        /// </summary>
        [JsonProperty("percentage_month")]
        public string PercentageMonth { get; set; } = string.Empty;

        /// <summary>
        /// Year change in percent.
        /// </summary>
        [JsonProperty("percentage_year")]
        public string PercentageYear { get; set; } = string.Empty;

        /// <summary>
        /// Date of the quote.
        /// </summary>
        [JsonProperty("date")]
        public string Date { get; set; } = string.Empty;
    }
}
