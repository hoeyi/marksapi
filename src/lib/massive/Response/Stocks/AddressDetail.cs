using System;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Stocks;

/// <summary>
/// Represents company headquarters address details.
/// </summary>
public class AddressDetail
{
    /// <summary>
    /// The address line 1.
    /// </summary>
    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    /// <summary>
    /// The address line 2.
    /// </summary>
    [JsonPropertyName("address2")]
    public string? Address2 { get; set; }

    /// <summary>
    /// The city.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>
    /// The country.
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    /// <summary>
    /// The postal code.
    /// </summary>
    [JsonPropertyName("postal_code")]
    public string? PostalCode { get; set; }

    /// <summary>
    /// The state or province.
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }
}
