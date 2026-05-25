using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the response data from the <b><em>/splits</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record SplitItem
    {
        /// <summary>
        /// Ticker symbol the split applies to.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Date of the split.
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Split ratio applied on the date.
        /// </summary>
        [JsonProperty("split_factor")]
        public double SplitFactor { get; set ; }
    }
}

