using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response.Economy
{
    /// <summary>
    /// Represents U.S. Treasury yield data for various maturity periods.
    /// Contains market yields on Treasury securities at constant maturities, quoted on an investment basis.
    /// </summary>
    public class TreasuryYield
    {
        /// <summary>
        /// Calendar date of the yield observation in YYYY-MM-DD format.
        /// </summary>
        [JsonPropertyName("date")]
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 1-Month Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_1_month")]
        [JsonProperty(PropertyName = "yield_1_month")]
        public double? Yield1Month { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 3-Month Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_3_month")]
        [JsonProperty(PropertyName = "yield_3_month")]
        public double? Yield3Month { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 6-Month Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_6_month")]
        [JsonProperty(PropertyName = "yield_6_month")]
        public double? Yield6Month { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 1-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_1_year")]
        [JsonProperty(PropertyName = "yield_1_year")]
        public double? Yield1Year { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 2-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_2_year")]
        [JsonProperty(PropertyName = "yield_2_year")]
        public double? Yield2Year { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 3-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_3_year")]
        [JsonProperty(PropertyName = "yield_3_year")]
        public double? Yield3Year { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 5-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_5_year")]
        [JsonProperty(PropertyName = "yield_5_year")]
        public double? Yield5Year { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 7-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_7_year")]
        [JsonProperty(PropertyName = "yield_7_year")]
        public double? Yield7Year { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 10-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_10_year")]
        [JsonProperty(PropertyName = "yield_10_year")]
        public double? Yield10Year { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 20-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_20_year")]
        [JsonProperty(PropertyName = "yield_20_year")]
        public double? Yield20Year { get; set; }

        /// <summary>
        /// Market Yield on U.S. Treasury Securities at 30-Year Constant Maturity, Quoted on an Investment Basis.
        /// </summary>
        [JsonPropertyName("yield_30_year")]
        [JsonProperty(PropertyName = "yield_30_year")]
        public double? Yield30Year { get; set; }

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