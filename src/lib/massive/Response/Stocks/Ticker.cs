using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks;

/// <summary>
/// Represents a single aggregate ticker response.
/// </summary>
[ExcludeFromCodeCoverage]
public class Ticker
{
/// <summary>
    /// Whether or not the asset is actively traded. False means the asset has been delisted.
    /// </summary>
    [JsonPropertyName("active")]
    [JsonProperty(PropertyName = "active")]
    public bool Active { get; set; }

    /// <summary>
    /// The name of the currency that this asset is priced against.
    /// </summary>
    [JsonPropertyName("base_currency_name")]
    [JsonProperty(PropertyName = "base_currency_name")]
    public string? BaseCurrencyName { get; set; }

    /// <summary>
    /// The ISO 4217 code of the currency that this asset is priced against.
    /// </summary>
    [JsonPropertyName("base_currency_symbol")]
    [JsonProperty(PropertyName = "base_currency_symbol")]
    public string? BaseCurrencySymbol { get; set; }

    /// <summary>
    /// The CIK number for this ticker.
    /// </summary>
    [JsonPropertyName("cik")]
    [JsonProperty(PropertyName = "cik")]
    public string? Cik { get; set; }

    /// <summary>
    /// The composite OpenFIGI number for this ticker.
    /// </summary>
    [JsonPropertyName("composite_figi")]
    [JsonProperty(PropertyName = "composite_figi")]
    public string? CompositeFigi { get; set; }

    /// <summary>
    /// The name of the currency that this asset is traded with.
    /// </summary>
    [JsonPropertyName("currency_name")]
    [JsonProperty(PropertyName = "currency_name")]
    public string? CurrencyName { get; set; }

    /// <summary>
    /// The ISO 4217 code of the currency that this asset is traded with.
    /// </summary>
    [JsonPropertyName("currency_symbol")]
    [JsonProperty(PropertyName = "currency_symbol")]
    public string? CurrencySymbol { get; set; }

    /// <summary>
    /// The last date that the asset was traded.
    /// </summary>
    [JsonPropertyName("delisted_utc")]
    [JsonProperty(PropertyName = "delisted_utc")]
    public string? DelistedUtc { get; set; }

    /// <summary>
    /// The information is accurate up to this time.
    /// </summary>
    [JsonPropertyName("last_updated_utc")]
    [JsonProperty(PropertyName = "last_update_utc")]
    public string? LastUpdatedUtc { get; set; }

    /// <summary>
    /// The locale of the asset. Valid values: us, global.
    /// </summary>
    [JsonPropertyName("locale")]
    [JsonProperty(PropertyName = "locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// The market type of the asset. Valid values: stocks, crypto, fx, otc, indices.
    /// </summary>
    [JsonPropertyName("market")]
    [JsonProperty(PropertyName = "market")]
    public string? Market { get; set; }

    /// <summary>
    /// The name of the asset. For stocks/equities this will be the company's registered name. 
    /// For crypto/fx this will be the name of the currency or coin pair.
    /// </summary>
    [JsonPropertyName("name")]
    [JsonProperty(PropertyName = "name")]
    public string? Name { get; set; }

    /// <summary>
    /// The ISO code of the primary listing exchange for this asset.
    /// </summary>
    [JsonPropertyName("primary_exchange")]
    [JsonProperty(PropertyName = "primary_exchange")]
    public string? PrimaryExchange { get; set; }

    /// <summary>
    /// The share class OpenFIGI number for this ticker.
    /// </summary>
    [JsonPropertyName("share_class_figi")]
    [JsonProperty(PropertyName = "share_class_figi")]
    public string? ShareClassFigi { get; set; }

    /// <summary>
    /// The exchange symbol that this item is traded under.
    /// </summary>
    [JsonPropertyName("ticker")]
    [JsonProperty(PropertyName = "ticker")]
    public string? Symbol { get; set; }

    /// <summary>
    /// The type of the asset.
    /// </summary>
    [JsonPropertyName("type")]
    [JsonProperty(PropertyName = "type")]
    public string? Type { get; set; }
}
