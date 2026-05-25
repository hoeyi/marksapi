using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the response data from the <b><em>/bondlist</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record BondListItem
    {
        /// <summary>
        /// Country supported for bonds.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; } = string.Empty;
    }
}

