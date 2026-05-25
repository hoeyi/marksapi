using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the response data from the <b><em>/tickers</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Ticker
    {
        /// <summary>
        /// Company or instrument name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ticker symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if intraday data is available.
        /// </summary>
        [JsonPropertyName("has_intraday")]
        public bool HasIntraday { get; set; }

        /// <summary>
        /// Ex-dividend date.
        /// </summary>
        [JsonPropertyName("has_eod")]
        public bool HasEod { get; set; }

        /// <summary>
        /// Country of the ticker.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set ; }

        /// <summary>
        /// List of exchanges the ticker is listed on.
        /// </summary>
        [JsonPropertyName("stock_exchanges")]
        public TickerExchange[] Exchanges { get; set; } = [];
    }

    /// <summary>
    /// Represents the child response data from the <b><em>/tickers</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record TickerExchange
    {
        /// <summary>
        /// Exchange name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Exchange acronym.
        /// </summary>
        [JsonPropertyName("acronym")]
        public string Acronym { get; set; } = string.Empty;

        /// <summary>
        /// MIC identification of the exchange.
        /// </summary>
        [JsonPropertyName("mic")]
        public string MIC { get; set; } = string.Empty;

        /// <summary>
        /// Exchange country.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Three-letter country code.
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Exchange city.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Exchange website URL.
        /// </summary>
        [JsonPropertyName("website")]
        public string Website { get; set; } = string.Empty;
    }
}