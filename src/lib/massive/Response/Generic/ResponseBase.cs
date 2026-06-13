using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Generic;

/// <summary>
/// Represents the base response attributes, namely unique identifier and request status.
/// </summary>
[ExcludeFromCodeCoverage]
public class ResponseBase
{
    /// <summary>
    /// A request id assigned by the server.
    /// </summary>
    [JsonPropertyName("request_id")]
    [JsonProperty(PropertyName = "request_id")]
    public required string RequestId { get; set; }

    /// <summary>
    /// The status of this request's response.
    /// </summary>
    [JsonPropertyName("status")]
    [JsonProperty(PropertyName = "status")]
    public required string Status { get; set; }
}
