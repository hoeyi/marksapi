using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("data")]
        public T[] Data { get; set; } = default!;

        /// <summary>
        /// Gets or sets the response pagination data.
        /// </summary>
        [JsonPropertyName("pagination")]
        public Pagination Pagination { get; set; } = default!;
    }

    /// <summary>
    /// Represents the resonse body from the '/eod' endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record EodResponse : GenericArrayResponse<EodBar>
    {
    }

    /// <summary>
    /// Represents the resonse body from the '/intraday' endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record IntradayResponse : GenericArrayResponse<EodBar>
    {
    }
}