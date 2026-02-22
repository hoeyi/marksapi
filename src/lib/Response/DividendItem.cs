using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the response data from the <b><em>/dividends</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Dividenditem
    {
        /// <summary>
        /// Ticker symbol the dividend applies to.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Ex-dividend date.
        /// </summary>
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Dividend amount per share.
        /// </summary>
        [JsonProperty("dividend")]
        public double Dividend { get; set ; }
    }
}

