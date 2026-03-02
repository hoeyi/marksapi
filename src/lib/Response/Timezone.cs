using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

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
        [JsonProperty("timezone")]
        public string Zone { get; set; } = string.Empty;

        /// <summary>
        /// Standard time abbreviation.
        /// </summary>
        [JsonProperty("abbr_dst")]
        public string Abbreviation { get; set; } = string.Empty;

        /// <summary>
        /// Daylight saving time abbreviation.
        /// </summary>
        [JsonProperty("abbr_dst")]
        public double AbbreviationDST { get; set ; }
    }
}

