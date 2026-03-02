using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    [ExcludeFromCodeCoverage]
    public record StockSplitData
    {
        /// <summary>
        /// Date of the split.
        /// </summary>
        [JsonProperty("date")] 
        public string Date { get; set; } = default!;

        /// <summary>
        /// Ticker symbol the split applies to.
        /// </summary>
        [JsonProperty("symbol")] 
        public string Symbol { get; set; } = default!;

        /// <summary>
        /// Split ratio applied on the date.
        /// </summary>
        [JsonProperty("split_factor")] 
        public decimal SplitFactor { get; set; } = default!;
    }
}