using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

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
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Acronym of the stock exchange.
        /// </summary>
        [JsonProperty("acronym")]
        public string Acronym { get; set; } = string.Empty;

        /// <summary>
        /// MIC identification of the exchange.
        /// </summary>
        [JsonProperty("mic")]
        public string MIC { get; set; } = string.Empty;

        /// <summary>
        /// Country of the stock exchange.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// Three-letter country code of the exchange.
        /// </summary>
        [JsonProperty("country_code")]
        public string CountryCode { get; set; } = string.Empty;

        /// <summary>
        /// City where the exchange is located.
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Website URL of the exchange.
        /// </summary>
        [JsonProperty("website")]
        public string Website { get; set; } = string.Empty;

        /// <summary>
        /// Operating Market Identifier Code.
        /// </summary>
        [JsonProperty("operating_mic")]
        public string OperatingMIC { get; set; } = string.Empty;

        /// <summary>
        /// Indicates operating MIC vs market segment MIC.
        /// </summary>
        [JsonProperty("oprt_sgmt")]
        public string OperatingSegment { get; set; } = string.Empty;

        /// <summary>
        /// Legal entity name.
        /// </summary>
        [JsonProperty("legal_entity_name")]
        public string LegalEntityName { get; set; } = string.Empty;

        /// <summary>
        /// Exchange Legal Entity Identifier (LEI).
        /// </summary>
        [JsonProperty("exchange_lei")]
        public string LEI { get; set; } = string.Empty;

        /// <summary>
        /// Market category code.
        /// </summary>
        [JsonProperty("market_category_code")]
        public string MarketCategoryCode { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the exchange.
        /// </summary>
        [JsonProperty("exchange_status")]
        public string ExchangeStatus { get; set; } = string.Empty;

        /// <summary>
        /// Creation date.
        /// </summary>
        [JsonProperty("date_creation")]
        public string DateCreation { get; set; } = string.Empty;

        /// <summary>
        /// Last update date.
        /// </summary>
        [JsonProperty("date_last_update")]
        public string DateLastUpdate { get; set; } = string.Empty;

        /// <summary>
        /// Last validation date.
        /// </summary>
        [JsonProperty("date_last_validation")]
        public string DateLastValidation { get; set; } = string.Empty;

        /// <summary>
        /// Expiry date.
        /// </summary>
        [JsonProperty("date_expiry")]
        public string DateExpiry { get; set; } = string.Empty;

        /// <summary>
        /// Additional comments.
        /// </summary>
        [JsonProperty("comments")]
        public string Comments { get; set; } = string.Empty;
    }
}
