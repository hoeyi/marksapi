using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Date of the split.
        /// </summary>
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Split ratio applied on the date.
        /// </summary>
        [JsonPropertyName("split_factor")]
        public double SplitFactor { get; set ; }
    }
}

