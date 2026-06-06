using System;
using System.Text.Json.Serialization;
using ApiClient.Massive.Response.Generic;

namespace ApiClient.Massive.Response.Stocks;

public class AggregateTickerResponse : CollectionResponse<Ticker>
{
    /// <summary>
    /// The total number of results for this request.
    /// </summary>
    [JsonPropertyName("count")]
    [JsonProperty(PropertyName = "count")]
    public int Count { get; set; }
}
