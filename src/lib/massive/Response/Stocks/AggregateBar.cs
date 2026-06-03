using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response
{
    /// <summary>
    /// Represents a single aggregate bar (candle) containing OHLCV data.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AggregateBar
    {
        /// <summary>
        /// The close price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("c")]
        public decimal Close { get; set; }

        /// <summary>
        /// The highest price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("h")]
        public decimal High { get; set; }

        /// <summary>
        /// The lowest price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("l")]
        public decimal Low { get; set; }

        /// <summary>
        /// The number of transactions in the aggregate window.
        /// </summary>
        [JsonPropertyName("n")]
        public int TradeCount { get; set; }

        /// <summary>
        /// The open price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("o")]
        public decimal Open { get; set; }

        /// <summary>
        /// Whether or not this aggregate is for an OTC ticker. This field will be left off if false.
        /// </summary>
        [JsonPropertyName("otc")]
        public bool? Otc { get; set; }

        /// <summary>
        /// The Unix millisecond timestamp for the start of the aggregate window.
        /// </summary>
        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

        /// <summary>
        /// The trading volume of the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("v")]
        public decimal Volume { get; set; }

        /// <summary>
        /// The volume weighted average price.
        /// </summary>
        [JsonPropertyName("vw")]
        public decimal VolumeWeightedAveragePrice { get; set; }
    }
}