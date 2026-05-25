using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Currency name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Currency symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Native currency symbol.
        /// </summary>
        [JsonPropertyName("symbol_native")]
        public string SymbolNative { get; set; } = string.Empty;
    }
}

