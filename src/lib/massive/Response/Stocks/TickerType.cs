using System;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks;

public class TickerType
{
    [JsonPropertyName("asset_class")]
    public required string AssetClass { get; set; }

    [JsonPropertyName("code")]
    public required string Code { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("locale")]
    public required string Locale { get; set; }
}
