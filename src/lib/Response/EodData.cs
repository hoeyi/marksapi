using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents the response data from the <b><em>/eod</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record EodData
    {
        /// <summary>
        /// Ticker symbol.
        /// </summary>
        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Company name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// MIC of the exchange.
        /// </summary>
        [JsonProperty("exchange")]
        public string Exchange { get; set; } = string.Empty;

        /// <summary>
        /// Human-friendly exchange code (for example NASDAQ).
        /// </summary>
        [JsonProperty("exchange_code")]
        public string ExchangeCode { get; set; } = string.Empty;

        /// <summary>
        /// Asset class.
        /// </summary>
        [JsonProperty("asset_type")]
        public string AssetType { get; set; } = string.Empty;

        /// <summary>
        /// Price currency (ISO code in lower case).
        /// </summary>
        [JsonProperty("price_currency")]
        public string PriceCurrency { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of the bar in ISO8601 with timezone.
        /// </summary>      
        [JsonProperty("date")]
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// Closing price of the session.
        /// </summary>  
        [JsonProperty("close")]
        public double Close { get; set; }

        /// <summary>
        /// Opening price for the trading session.
        /// </summary>
        [JsonProperty("open")]
        public double Open { get; set; }

        /// <summary>
        /// Highest price of the session.
        /// </summary>
        [JsonProperty("high")]
        public double High { get; set; }

        /// <summary>
        /// Lowest price of the session.
        /// </summary>          
        [JsonProperty("low")]
        public double Low { get; set; }

        /// <summary>
        /// Traded volume during the session.
        /// </summary>  
        [JsonProperty("volume")]
        public long Volume { get; set; }

        /// <summary>
        /// Close price adjusted for corporate actions.
        /// </summary>  
        [JsonProperty("adj_close")]
        public double AdjClose { get; set; }

        /// <summary>
        /// Open price adjusted for corporate actions.
        /// </summary>  
        [JsonProperty("adj_open")]
        public double AdjOpen { get; set; }

        /// <summary>
        /// High price adjusted for corporate actions.
        /// </summary>  
        [JsonProperty("adj_high")]
        public double AdjHigh { get; set; }

        /// <summary>
        /// Low price adjusted for corporate actions.
        /// </summary>  
        [JsonProperty("adj_low")]
        public double AdjLow { get; set; }

        /// <summary>
        /// Volume adjusted for corporate actions.
        /// </summary>  
        [JsonProperty("adj_volume")]
        public long AdjVolume { get; set; }

        /// <summary>
        /// Cumulative stock split factor applied for the date.
        /// </summary>  
        [JsonProperty("split_factor")]
        public double SplitFactor { get; set; }

        /// <summary>
        /// Dividend amount per share for the date.
        /// </summary>  
        [JsonProperty("dividend")]
        public double Dividend { get; set; }
    }    
}