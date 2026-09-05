using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Economy
{
    /// <summary>
    /// Represents a single observation of inflation indicators for a given date.
    /// Contains both headline and core inflation measures from CPI and PCE indexes.
    /// </summary>
    public class Inflation
    {
        /// <summary>
        /// Consumer Price Index (CPI) for All Urban Consumers — a standard measure of headline inflation based on a fixed basket of goods and services, not seasonally adjusted.
        /// </summary>
        [JsonPropertyName("cpi")]
        [JsonProperty(PropertyName = "cpi")]
        public double? Cpi { get; set; }

        /// <summary>
        /// Core Consumer Price Index — the CPI excluding food and energy, used to understand underlying inflation trends without short-term volatility.
        /// </summary>
        [JsonPropertyName("cpi_core")]
        [JsonProperty(PropertyName = "cpi_core")]
        public double? CpiCore { get; set; }

        /// <summary>
        /// Year-over-year percentage change in the headline CPI — the most commonly cited inflation rate in public discourse and economic policy.
        /// </summary>
        [JsonPropertyName("cpi_year_over_year")]
        [JsonProperty(PropertyName = "cpi_year_over_year")]
        public double? CpiYearOverYear { get; set; }

        /// <summary>
        /// Calendar date of the observation in YYYY-MM-DD format.
        /// </summary>
        [JsonPropertyName("date")]
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// Personal Consumption Expenditures (PCE) Price Index — a broader measure of inflation used by the Federal Reserve, reflecting actual consumer spending patterns and updated basket weights.
        /// </summary>
        [JsonPropertyName("pce")]
        [JsonProperty(PropertyName = "pce")]
        public double? Pce { get; set; }

        /// <summary>
        /// Core PCE Price Index — excludes food and energy prices from the PCE index, and is the Fed's preferred measure of underlying inflation.
        /// </summary>
        [JsonPropertyName("pce_core")]
        [JsonProperty(PropertyName = "pce_core")]
        public double? PceCore { get; set; }

        /// <summary>
        /// Nominal Personal Consumption Expenditures — total dollar value of consumer spending in the U.S. economy, reported in billions of dollars and not adjusted for inflation.
        /// </summary>
        [JsonPropertyName("pce_spending")]
        [JsonProperty(PropertyName = "pce_spending")]
        public double? PceSpending { get; set; }

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

}