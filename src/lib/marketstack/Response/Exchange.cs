using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace MarketStackApi
{
    /// <summary>
    /// Represents the response data from the <b><em>/exchanges</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Exchange
    {
        /// <summary>
        /// Name of the stock exchange.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Acronym of the stock exchange.
        /// </summary>
        [JsonPropertyName("acronym")]
        public string Acronym { get; set; } = string.Empty;

        /// <summary>
        /// MIC identification of the exchange.
        /// </summary>
        [JsonPropertyName("mic")]
        public string MIC { get; set; } = string.Empty;

        /// <summary>
        /// Country of the stock exchange.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Three-letter country code of the exchange.
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// City where the exchange is located.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Website URL of the exchange.
        /// </summary>
        [JsonPropertyName("website")]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Operating Market Identifier Code.
        /// </summary>
        [JsonPropertyName("operating_mic")]
        public string OperatingMIC { get; set; } = string.Empty;

        /// <summary>
        /// Indicates operating MIC vs market segment MIC.
        /// </summary>
        [JsonPropertyName("oprt_sgmt")]
        public string OperatingSegment { get; set; } = string.Empty;

        /// <summary>
        /// Legal entity name.
        /// </summary>
        [JsonPropertyName("legal_entity_name")]
        public string LegalEntityName { get; set; } = string.Empty;

        /// <summary>
        /// Exchange Legal Entity Identifier (LEI).
        /// </summary>
        [JsonPropertyName("exchange_lei")]
        public string LEI { get; set; } = string.Empty;

        /// <summary>
        /// Market category code.
        /// </summary>
        [JsonPropertyName("market_category_code")]
        public string MarketCategoryCode { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the exchange.
        /// </summary>
        [JsonPropertyName("exchange_status")]
        public string ExchangeStatus { get; set; } = string.Empty;

        /// <summary>
        /// Creation date.
        /// </summary>
        [JsonPropertyName("date_creation")]
        public string DateCreation { get; set; } = string.Empty;

        /// <summary>
        /// Last update date.
        /// </summary>
        [JsonPropertyName("date_last_update")]
        public string DateLastUpdate { get; set; } = string.Empty;

        /// <summary>
        /// Last validation date.
        /// </summary>
        [JsonPropertyName("date_last_validation")]
        public string DateLastValidation { get; set; } = string.Empty;

        /// <summary>
        /// Expiry date.
        /// </summary>
        [JsonPropertyName("date_expiry")]
        public string DateExpiry { get; set; } = string.Empty;

        /// <summary>
        /// Additional comments.
        /// </summary>
        [JsonPropertyName("comments")]
        public string Comments { get; set; } = string.Empty;
    }
}
