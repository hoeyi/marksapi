using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ApiClient.Massive.Response.Generic;

namespace ApiClient.Massive.Response.Stocks
{
    /// <summary>
    /// Represents the complete response data from a stock aggregate endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AggregateBarResponse : CollectionResponse<AggregateBar>
    {
        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("ticker")]
        [JsonProperty(PropertyName = "ticker")]
        public required string Ticker { get; set; }

        /// <summary>
        /// Whether or not this response was adjusted for splits.
        /// </summary>
        [JsonPropertyName("adjusted")]
        [JsonProperty(PropertyName = "adjusted")]
        public bool Adjusted { get; set; }

        /// <summary>
        /// The number of aggregates (minute or day) used to generate the response.
        /// </summary>
        [JsonPropertyName("queryCount")]
        [JsonProperty(PropertyName = "queryCount")]
        public int QueryCount { get; set; }

        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("resultsCount")]
        [JsonProperty(PropertyName = "resultsCount")]
        public int ResultsCount { get; set; }
    }
}