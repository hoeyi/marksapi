using System.Diagnostics.CodeAnalysis;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the resonse body from the '/splits'endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record StockSplitResponse() : GenericArrayResponse<StockSplitData>
    {
    }
}
