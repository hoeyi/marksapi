using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the child response data from the <b><em>//timezones</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record Timezone
    {
        /// <summary>
        /// IANA timezone name.
        /// </summary>
        [JsonPropertyName("timezone")]
        public string Zone { get; set; } = string.Empty;

        /// <summary>
        /// Standard time abbreviation.
        /// </summary>
        [JsonPropertyName("abbr_dst")]
        public string Abbreviation { get; set; } = string.Empty;

        /// <summary>
        /// Daylight saving time abbreviation.
        /// </summary>
        [JsonPropertyName("abbr_dst")]
        public double AbbreviationDST { get; set ; }
    }
}

