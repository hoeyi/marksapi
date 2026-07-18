using System.Text.Json.Serialization;
using ApiClient.Massive.Response.Generic;

namespace ApiClient.Massive.Response;

/// <summary>
/// Represents the response from the Massive API endpoint for retrieving ticker / asset overviews.
/// </summary>
public class TickerOverviewResponse : ResponseBase
{
    /// <summary>
    /// Gets or sets the overview object for this response.
    /// </summary>
    [JsonPropertyName("results")]
    [JsonProperty(PropertyName = "results")]
    public TickerOverview? Results { get; set; }
}

