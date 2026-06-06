using System;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks;

public class TickerType
{
    [JsonPropertyName("asset_class")]
    [JsonProperty(PropertyName = "asset_class")]
    public required string AssetClass { get; set; }

    [JsonPropertyName("code")]
    [JsonProperty(PropertyName = "code")]
    public required string Code { get; set; }

    [JsonPropertyName("description")]
    [JsonProperty(PropertyName = "description")]
    public required string Description { get; set; }

    [JsonPropertyName("locale")]
    [JsonProperty(PropertyName = "locale")]
    public required string Locale { get; set; }
}
