using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Generic
{
    /// <summary>
    /// Represents the complete response data from a generic aggregate endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AggregateResponse<T> : ResponseBase
        where T : class
    {
        /// <summary>
        /// If present, this value can be used to fetch the next page of data.
        /// </summary>
        [JsonPropertyName("next_url")]
        public string? NextUrl { get; set; }

        /// <summary>
        /// The total number of results for this request.
        /// </summary>
        [JsonPropertyName("resultsCount")]
        public int ResultsCount { get; set; }

        /// <summary>
        /// An array of results containing the requested data.
        /// </summary>
        [JsonPropertyName("results")]
        public List<T> Results { get; set; } = [];
    }
}