using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ApiClient.Massive.Response.Generic;

namespace ApiClient.Massive.Response
{
    /// <summary>
    /// Represents the complete response data from a stock aggregate endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AggregateBarResponse : AggregateResponse<AggregateBar>
    {
        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Whether or not this response was adjusted for splits.
        /// </summary>
        [JsonPropertyName("adjusted")]
        public bool Adjusted { get; set; }

        /// <summary>
        /// The number of aggregates (minute or day) used to generate the response.
        /// </summary>
        [JsonPropertyName("queryCount")]
        public int QueryCount { get; set; }
    }
}