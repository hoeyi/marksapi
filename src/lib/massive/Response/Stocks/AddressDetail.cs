using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks
{
    /// <summary>
    /// Represents company headquarters address details.
    /// </summary>
    public class AddressDetail
    {
        /// <summary>
        /// The address line 1.
        /// </summary>
        [JsonPropertyName("address1")]
        [JsonProperty(PropertyName = "address1")]
        public string? Address1 { get; set; }

        /// <summary>
        /// The address line 2.
        /// </summary>
        [JsonPropertyName("address2")]
        [JsonProperty(PropertyName = "address2")]
        public string? Address2 { get; set; }

        /// <summary>
        /// The city.
        /// </summary>
        [JsonPropertyName("city")]
        [JsonProperty(PropertyName = "city")]
        public string? City { get; set; }

        /// <summary>
        /// The country.
        /// </summary>
        [JsonPropertyName("country")]
        [JsonProperty(PropertyName = "country")]
        public string? Country { get; set; }

        /// <summary>
        /// The postal code.
        /// </summary>
        [JsonPropertyName("postal_code")]
        [JsonProperty(PropertyName = "postal_code")]
        public string? PostalCode { get; set; }

        /// <summary>
        /// The state or province.
        /// </summary>
        [JsonPropertyName("state")]
        [JsonProperty(PropertyName = "state")]
        public string? State { get; set; }
    }
}

