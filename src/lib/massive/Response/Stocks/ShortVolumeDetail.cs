using System;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks;

/// <summary>
/// Represents a single short volume data point for a ticker on a specific date.
/// </summary>
public class ShortVolumeDetail
{
    /// <summary>
    /// Short volume reported via the Alternative Display Facility (ADF), excluding exempt volume.
    /// </summary>
    [JsonPropertyName("adf_short_volume")]
    [JsonProperty(PropertyName = "adf_short_volume")]
    public long AdfShortVolume { get; set; }

    /// <summary>
    /// Short volume reported via ADF that was marked as exempt.
    /// </summary>
    [JsonPropertyName("adf_short_volume_exempt")]
    [JsonProperty(PropertyName = "adf_short_volume_exempt")]
    public long AdfShortVolumeExempt { get; set; }

    /// <summary>
    /// The date of trade activity reported in the format YYYY-MM-DD.
    /// </summary>
    [JsonPropertyName("date")]
    [JsonProperty(PropertyName = "date")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Portion of short volume that was marked as exempt from regulation SHO.
    /// </summary>
    [JsonPropertyName("exempt_volume")]
    [JsonProperty(PropertyName = "exempt_volume")]
    public decimal ExemptVolume { get; set; }

    /// <summary>
    /// Short volume reported from Nasdaq's Carteret facility, excluding exempt volume.
    /// </summary>
    [JsonPropertyName("nasdaq_carteret_short_volume")]
    [JsonProperty(PropertyName = "nasdaq_carteret_short_volume")]
    public long NasdaqCarteretShortVolume { get; set; }

    /// <summary>
    /// Short volume from Nasdaq Carteret that was marked as exempt.
    /// </summary>
    [JsonPropertyName("nasdaq_carteret_short_volume_exempt")]
    [JsonProperty(PropertyName = "nasdaq_carteret_short_volume_exempt")]
    public long NasdaqCarteretShortVolumeExempt { get; set; }

    /// <summary>
    /// Short volume reported from Nasdaq's Chicago facility, excluding exempt volume.
    /// </summary>
    [JsonPropertyName("nasdaq_chicago_short_volume")]
    [JsonProperty(PropertyName = "nasdaq_chicago_short_volume")]
    public long NasdaqChicagoShortVolume { get; set; }

    /// <summary>
    /// Short volume from Nasdaq Chicago that was marked as exempt.
    /// </summary>
    [JsonPropertyName("nasdaq_chicago_short_volume_exempt")]
    [JsonProperty(PropertyName = "nasdaq_chicago_short_volume_exempt")]
    public long NasdaqChicagoShortVolumeExempt { get; set; }

    /// <summary>
    /// Portion of short volume that was not exempt from regulation SHO (i.e., short_volume - exempt_volume).
    /// </summary>
    [JsonPropertyName("non_exempt_volume")]
    [JsonProperty(PropertyName = "non_exempt_volume")]
    public decimal NonExemptVolume { get; set; }

    /// <summary>
    /// Short volume reported from NYSE facilities, excluding exempt volume.
    /// </summary>
    [JsonPropertyName("nyse_short_volume")]
    [JsonProperty(PropertyName = "nyse_short_volume")]
    public long NyseShortVolume { get; set; }

    /// <summary>
    /// Short volume from NYSE facilities that was marked as exempt.
    /// </summary>
    [JsonPropertyName("nyse_short_volume_exempt")]
    [JsonProperty(PropertyName = "nyse_short_volume_exempt")]
    public long NyseShortVolumeExempt { get; set; }

    /// <summary>
    /// Total number of shares sold short across all venues for the ticker on the given date.
    /// </summary>
    [JsonPropertyName("short_volume")]
    [JsonProperty(PropertyName = "short_volume")]
    public decimal ShortVolume { get; set; }

    /// <summary>
    /// The percentage of total volume that was sold short. Calculated as (short_volume / total_volume) * 100.
    /// </summary>
    [JsonPropertyName("short_volume_ratio")]
    [JsonProperty(PropertyName = "short_volume_ratio")]
    public decimal ShortVolumeRatio { get; set; }

    /// <summary>
    /// The primary ticker symbol for the stock.
    /// </summary>
    [JsonPropertyName("ticker")]
    [JsonProperty(PropertyName = "ticker")]
    public required string Ticker { get; set; }

    /// <summary>
    /// Total reported volume across all venues for the ticker on the given date.
    /// </summary>
    [JsonPropertyName("total_volume")]
    [JsonProperty(PropertyName = "total_volume")]
    public decimal TotalVolume { get; set; }
}

