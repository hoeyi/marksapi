using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the response data from the <b><em>/currencies</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Currency
    {
        /// <summary>
        /// Three-letter currency code.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Currency name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Currency symbol.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Native currency symbol.
        /// </summary>
        [JsonProperty("symbol_native")]
        public string SymbolNative { get; set; } = string.Empty;
    }
}

