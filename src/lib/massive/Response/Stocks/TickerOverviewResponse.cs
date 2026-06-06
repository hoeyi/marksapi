using System;
using ApiClient.Massive.Response.Generic;

namespace ApiClient.Massive.Response.Stocks;

/// <summary>
/// Represents the response from the Massive API endpoint for retrieving ticker / asset overviews.
/// </summary>
public class TickerOverviewResponse : ResponseBase
{
    /// <summary>
    /// Gets or sets the overview object for this response.
    /// </summary>
    public TickerOverview? TickerOverview { get; set; }
}

