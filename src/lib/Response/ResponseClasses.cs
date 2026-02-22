using System.Diagnostics.CodeAnalysis;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the resonse body from the '/eod'endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record EodResponse : GenericArrayResponse<EodBar>
    {
    }
}