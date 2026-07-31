using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Economy;

/// <summary>
/// Represents a single observation of labor market indicators for a given date.
/// Contains unemployment, participation rate, earnings, and job openings metrics.
/// </summary>
public class LaborMarket
{
    /// <summary>
    /// Average hourly earnings of all employees on private nonfarm payrolls in USD (CES0500000003 series from FRED).
    /// </summary>
    [JsonPropertyName("avg_hourly_earnings")]
    public double? AvgHourlyEarnings { get; set; }

    /// <summary>
    /// Calendar date of the observation in YYYY-MM-DD format.
    /// </summary>
    [JsonPropertyName("date")]
    public string Date { get; set; } = string.Empty;

    /// <summary>
    /// Total nonfarm job openings in thousands (JTSJOL series from FRED).
    /// </summary>
    [JsonPropertyName("job_openings")]
    public double? JobOpenings { get; set; }

    /// <summary>
    /// Civilian labor force participation rate as a percentage of the civilian noninstitutional population (CIVPART series from FRED).
    /// </summary>
    [JsonPropertyName("labor_force_participation_rate")]
    public double? LaborForceParticipationRate { get; set; }

    /// <summary>
    /// Civilian unemployment rate as a percentage of the labor force (UNRATE series from FRED).
    /// </summary>
    [JsonPropertyName("unemployment_rate")]
    public double? UnemploymentRate { get; set; }


    #region Non-json properties
    /// <summary>
    /// The status of this request's response.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string? Status { get; set; }

    /// <summary>
    /// A request id assigned by the server.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string? RequestId { get; set; }
    #endregion      
}