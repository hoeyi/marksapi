using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Marketstack
{
    /// <summary>
    /// Represents generic response for price responses.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record QuoteBar
    {
        /// <summary>
        /// Ticker symbol.
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        /// <summary>
        /// Company name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// MIC of the exchange.
        /// </summary>
        [JsonPropertyName("exchange")]
        public string Exchange { get; set; } = string.Empty;

        /// <summary>
        /// Human-friendly exchange code (for example NASDAQ).
        /// </summary>
        [JsonPropertyName("exchange_code")]
        public string ExchangeCode { get; set; } = string.Empty;

        /// <summary>
        /// Asset class.
        /// </summary>
        [JsonPropertyName("asset_type")]
        public string AssetType { get; set; } = string.Empty;

        /// <summary>
        /// Price currency (ISO code in lower case).
        /// </summary>
        [JsonPropertyName("price_currency")]
        public string PriceCurrency { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of the bar in ISO8601 with timezone.
        /// </summary>      
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// Closing price of the session.
        /// </summary>  
        [JsonPropertyName("close")]
        public double Close { get; set; }

        /// <summary>
        /// Opening price for the trading session.
        /// </summary>
        [JsonPropertyName("open")]
        public double Open { get; set; }

        /// <summary>
        /// Highest price of the session.
        /// </summary>
        [JsonPropertyName("high")]
        public double High { get; set; }

        /// <summary>
        /// Lowest price of the session.
        /// </summary>          
        [JsonPropertyName("low")]
        public double Low { get; set; }

        /// <summary>
        /// Traded volume during the session.
        /// </summary>  
        [JsonPropertyName("volume")]
        public long Volume { get; set; }

        /// <summary>
        /// Close price adjusted for corporate actions.
        /// </summary>  
        [JsonPropertyName("adj_close")]
        public double AdjClose { get; set; }

        /// <summary>
        /// Open price adjusted for corporate actions.
        /// </summary>  
        [JsonPropertyName("adj_open")]
        public double AdjOpen { get; set; }

        /// <summary>
        /// High price adjusted for corporate actions.
        /// </summary>  
        [JsonPropertyName("adj_high")]
        public double AdjHigh { get; set; }

        /// <summary>
        /// Low price adjusted for corporate actions.
        /// </summary>  
        [JsonPropertyName("adj_low")]
        public double AdjLow { get; set; }

        /// <summary>
        /// Volume adjusted for corporate actions.
        /// </summary>  
        [JsonPropertyName("adj_volume")]
        public long AdjVolume { get; set; }

        /// <summary>
        /// Cumulative stock split factor applied for the date.
        /// </summary>  
        [JsonPropertyName("split_factor")]
        public double SplitFactor { get; set; }

        /// <summary>
        /// Dividend amount per share for the date.
        /// </summary>  
        [JsonPropertyName("dividend")]
        public double Dividend { get; set; }
    }    

    /// <summary>
    /// Represents the response data from the <b><em>/eod</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record EodBar : QuoteBar
    {
    }

    /// <summary>
    /// Represents the response data from the <b><em>/intraday</em></b> endpoint.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public record IntradayBar : QuoteBar
    {
    }
}