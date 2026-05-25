using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Marketstack
{
    [ExcludeFromCodeCoverage]
    public record StockSplitData
    {
        /// <summary>
        /// Date of the split.
        /// </summary>
        [JsonPropertyName("date")] 
        public string Date { get; set; } = default!;

        /// <summary>
        /// Ticker symbol the split applies to.
        /// </summary>
        [JsonPropertyName("symbol")] 
        public string Symbol { get; set; } = default!;

        /// <summary>
        /// Split ratio applied on the date.
        /// </summary>
        [JsonPropertyName("split_factor")] 
        public decimal SplitFactor { get; set; } = default!;
    }
}