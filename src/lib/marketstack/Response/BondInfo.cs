using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MarketStackApi
{
    /// <summary>
    /// Represents the response data from the <b><em>/bond</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BondInfo
    {
        /// <summary>
        /// Region where the bond is supported.
        /// </summary>
        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Country where the bond is supported.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Bond tenor/type (for example 10Y).
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Current bond yield.
        /// </summary>
        [JsonPropertyName("yield")]
        public string Yield { get; set; } = string.Empty;

        /// <summary>
        /// Price change day-over-day.
        /// </summary>
        [JsonPropertyName("price_change_day")]
        public string PriceChangeDay { get; set; } = string.Empty;

        /// <summary>
        /// Weekly change percentage.
        /// </summary>
        [JsonPropertyName("percentage_week")]
        public string PercentageWeek { get; set; } = string.Empty;

        /// <summary>
        /// Monthly change percentage.
        /// </summary>
        [JsonPropertyName("percentage_month")]
        public string PercentageMonth { get; set; } = string.Empty;

        /// <summary>
        /// Yearly change percentage.
        /// </summary>
        [JsonPropertyName("percentage_year")]
        public string PercentageYear { get; set; } = string.Empty;

        /// <summary>
        /// Quote date.
        /// </summary>
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;
    }
}
