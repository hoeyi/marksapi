using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks
{
    /// <summary>
    /// Represents the type descriptors of a ticker.
    /// </summary>
    public class TickerType
    {
        /// <summary>
        /// An identifier for a group of similar financial instruments.
        /// </summary>
        [JsonPropertyName("asset_class")]
        [JsonProperty(PropertyName = "asset_class")]
        public required string AssetClass { get; set; }

        /// <summary>
        /// A code used by Massive to refer to this ticker type.
        /// </summary>
        [JsonPropertyName("code")]
        [JsonProperty(PropertyName = "code")]
        public required string Code { get; set; }

        /// <summary>
        /// A short description of this ticker type.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonProperty(PropertyName = "description")]
        public required string Description { get; set; }

        /// <summary>
        /// An identifier for a geographical location.
        /// </summary>
        [JsonPropertyName("locale")]
        [JsonProperty(PropertyName = "locale")]
        public required string Locale { get; set; }
    }
}

