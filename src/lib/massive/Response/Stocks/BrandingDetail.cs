using System;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks;

/// <summary>
/// Represents branding details including URLs for visual identification.
/// </summary>
public class BrandingDetail
{
    /// <summary>
    /// URL to the company logo.
    /// </summary>
    [JsonPropertyName("logo_url")]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// URL to the company icon.
    /// </summary>
    [JsonPropertyName("icon_url")]
    public string? IconUrl { get; set; }
}

