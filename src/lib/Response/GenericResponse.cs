using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents a generic response with an array of <typeparamref name="T"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record GenericArrayResponse<T>
    {
        /// <summary>
        /// Gets or sets response <typeparamref name="T"/> data.
        /// </summary>
        [JsonProperty("data")]
        public T[] Data { get; set; } = default!;

        /// <summary>
        /// Gets or sets the response pagination data.
        /// </summary>
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; } = default!;
    }
}