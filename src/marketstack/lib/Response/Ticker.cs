using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

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
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Ticker symbol.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if intraday data is available.
        /// </summary>
        [JsonProperty("has_intraday")]
        public bool HasIntraday { get; set; }

        /// <summary>
        /// Ex-dividend date.
        /// </summary>
        [JsonProperty("has_eod")]
        public bool HasEod { get; set; }

        /// <summary>
        /// Country of the ticker.
        /// </summary>
        [JsonProperty("country")]
        public string? Country { get; set ; }

        /// <summary>
        /// List of exchanges the ticker is listed on.
        /// </summary>
        [JsonProperty("stock_exchanges")]
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
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Exchange acronym.
        /// </summary>
        [JsonProperty("acronym")]
        public string Acronym { get; set; } = string.Empty;

        /// <summary>
        /// MIC identification of the exchange.
        /// </summary>
        [JsonProperty("mic")]
        public string MIC { get; set; } = string.Empty;

        /// <summary>
        /// Exchange country.
        /// </summary>
        [JsonProperty("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Three-letter country code.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// Exchange city.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Exchange website URL.
        /// </summary>
        [JsonProperty("website")]
        public string Website { get; set; } = string.Empty;
    }
}