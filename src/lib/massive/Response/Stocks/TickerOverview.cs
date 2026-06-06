using System;
using System.Text.Json.Serialization;
using ApiClient.Massive.Response.Generic;

namespace ApiClient.Massive.Response.Stocks;


/// <summary>
/// Represents the overview of the asset identified by the ticker.
/// </summary>
public class TickerOverview
{
    /// <summary>
    /// Whether or not the asset is actively traded. False means the asset has been delisted.
    /// </summary>
    [JsonPropertyName("active")]
    public bool Active { get; set; }

    /// <summary>
    /// Company headquarters address details.
    /// </summary>
    [JsonPropertyName("address")]
    public AddressDetail? Address { get; set; }

    /// <summary>
    /// Provides URLs aiding in visual identification.
    /// </summary>
    [JsonPropertyName("branding")]
    public BrandingDetail? Branding { get; set; }

    /// <summary>
    /// The CIK number for this ticker.
    /// </summary>
    [JsonPropertyName("cik")]
    public string? Cik { get; set; }

    /// <summary>
    /// The composite OpenFIGI number for this ticker.
    /// </summary>
    [JsonPropertyName("composite_figi")]
    public string? CompositeFigi { get; set; }

    /// <summary>
    /// The name of the currency that this asset is traded with.
    /// </summary>
    [JsonPropertyName("currency_name")]
    public string? CurrencyName { get; set; }

    /// <summary>
    /// The last date that the asset was traded.
    /// </summary>
    [JsonPropertyName("delisted_utc")]
    public string? DelistedUtc { get; set; }

    /// <summary>
    /// A description of the company and what they do/offer.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The URL of the company's website homepage.
    /// </summary>
    [JsonPropertyName("homepage_url")]
    public string? HomepageUrl { get; set; }

    /// <summary>
    /// The date that the symbol was first publicly listed in the format YYYY-MM-DD.
    /// </summary>
    [JsonPropertyName("list_date")]
    public string? ListDate { get; set; }

    /// <summary>
    /// The locale of the asset.
    /// </summary>
    [JsonPropertyName("locale")]
    public string? Locale { get; set; }

    /// <summary>
    /// The market type of the asset.
    /// </summary>
    [JsonPropertyName("market")]
    public string? Market { get; set; }

    /// <summary>
    /// The most recent close price of the ticker multiplied by weighted outstanding shares.
    /// </summary>
    [JsonPropertyName("market_cap")]
    public decimal MarketCap { get; set; }

    /// <summary>
    /// The name of the asset. For stocks/equities this will be the companies registered name.
    /// For crypto/fx this will be the name of the currency or coin pair.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The phone number for the company behind this ticker.
    /// </summary>
    [JsonPropertyName("phone_number")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// The ISO code of the primary listing exchange for this asset.
    /// </summary>
    [JsonPropertyName("primary_exchange")]
    public string? PrimaryExchange { get; set; }

    /// <summary>
    /// Round lot size of this security.
    /// </summary>
    [JsonPropertyName("round_lot")]
    public int RoundLot { get; set; }

    /// <summary>
    /// The share Class OpenFIGI number for this ticker.
    /// </summary>
    [JsonPropertyName("share_class_figi")]
    public string? ShareClassFigi { get; set; }

    /// <summary>
    /// The recorded number of outstanding shares for this particular share class.
    /// </summary>
    [JsonPropertyName("share_class_shares_outstanding")]
    public long ShareClassSharesOutstanding { get; set; }

    /// <summary>
    /// The standard industrial classification code for this ticker.
    /// </summary>
    [JsonPropertyName("sic_code")]
    public string? SicCode { get; set; }

    /// <summary>
    /// A description of this ticker's SIC code.
    /// </summary>
    [JsonPropertyName("sic_description")]
    public string? SicDescription { get; set; }

    /// <summary>
    /// The exchange symbol that this item is traded under.
    /// </summary>
    [JsonPropertyName("ticker")]
    public string? Ticker { get; set; }

    /// <summary>
    /// The root of a specified ticker. For example, the root of BRK.A is BRK.
    /// </summary>
    [JsonPropertyName("ticker_root")]
    public string? TickerRoot { get; set; }

    /// <summary>
    /// The suffix of a specified ticker. For example, the suffix of BRK.A is A.
    /// </summary>
    [JsonPropertyName("ticker_suffix")]
    public string? TickerSuffix { get; set; }

    /// <summary>
    /// The approximate number of employees for the company.
    /// </summary>
    [JsonPropertyName("total_employees")]
    public int TotalEmployees { get; set; }

    /// <summary>
    /// The type of the asset.
    /// </summary>
    [JsonPropertyName("type")]
    public string? TypeCode { get; set; }

    /// <summary>
    /// The shares outstanding calculated assuming all shares of other share classes are converted to this share class.
    /// </summary>
    [JsonPropertyName("weighted_shares_outstanding")]
    public long WeightedSharesOutstanding { get; set; }
}
