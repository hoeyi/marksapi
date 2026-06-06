using System;

namespace ApiClient.Massive.Response.Stocks;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using ApiClient.Massive.Response.Generic;

/// <summary>
/// Represents the response from the Massive API endpoint for retrieving short volume data.
/// </summary>
public class ShortVolumeResponse : CollectionResponse<ShortVolumeResult>
{
}

