using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the response data from the <b><em>/indexlist</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record IndexListem
    {
        /// <summary>
        /// Benchmark code of the market index.
        /// </summary>
        [JsonPropertyName("benchmark")]
        public string Benchmark { get; set; } = string.Empty;
    }
}

