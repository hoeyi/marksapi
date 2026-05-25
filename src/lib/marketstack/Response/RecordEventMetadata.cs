using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents response metadata for various events, e.g., created, upated etc.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record RecordEventMetadata
    {
        /// <summary>
        /// Event date.
        /// </summary>
        [JsonProperty("date")]
        public virtual string Date { get; set; } = string.Empty;

        /// <summary>
        /// Timezone type.
        /// </summary>
        [JsonProperty("timezone_type")]
        public string TimezoneType { get; set; } = string.Empty;

        /// <summary>
        /// Timezone offset.
        /// </summary>
        [JsonProperty("timezone")]
        public string Timezone { get; set ; } = string.Empty;
    }
}

