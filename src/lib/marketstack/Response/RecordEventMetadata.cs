using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("date")]
        public virtual string Date { get; set; } = string.Empty;

        /// <summary>
        /// Timezone type.
        /// </summary>
        [JsonPropertyName("timezone_type")]
        public string TimezoneType { get; set; } = string.Empty;

        /// <summary>
        /// Timezone offset.
        /// </summary>
        [JsonPropertyName("timezone")]
        public string Timezone { get; set ; } = string.Empty;
    }
}

