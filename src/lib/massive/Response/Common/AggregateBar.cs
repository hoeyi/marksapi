using ApiClient.Massive.Response.Stocks;
using Newtonsoft.Json.Bson;
using System;
using System.ComponentModel;
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
        [JsonProperty(PropertyName = "c")]
        public decimal Close { get; set; }

        /// <summary>
        /// The highest price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("h")]
        [JsonProperty(PropertyName = "h")]
        public decimal High { get; set; }

        /// <summary>
        /// The lowest price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("l")]
        [JsonProperty(PropertyName = "l")]
        public decimal Low { get; set; }

        /// <summary>
        /// The number of transactions in the aggregate window.
        /// </summary>
        [JsonPropertyName("n")]
        [JsonProperty(PropertyName = "n")]
        public int TradeCount { get; set; }

        /// <summary>
        /// The open price for the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("o")]
        [JsonProperty(PropertyName = "o")]
        public decimal Open { get; set; }

        /// <summary>
        /// Whether or not this aggregate is for an OTC ticker. This field will be left off if false.
        /// </summary>
        [JsonPropertyName("otc")]
        [JsonProperty(PropertyName = "otc")]
        public bool? Otc { get; set; }

        /// <summary>
        /// The Unix millisecond timestamp for the start of the aggregate window.
        /// </summary>
        [JsonPropertyName("t")]
        [JsonProperty(PropertyName = "t")]
        public long Timestamp { get; set; }

        /// <summary>
        /// The trading volume of the symbol in the given time period.
        /// </summary>
        [JsonPropertyName("v")]
        [JsonProperty(PropertyName = "v")]
        public decimal Volume { get; set; }

        /// <summary>
        /// The volume weighted average price.
        /// </summary>
        [JsonPropertyName("vw")]
        [JsonProperty(PropertyName = "vw")]
        public decimal VolumeWeightedAveragePrice { get; set; }

        #region Non-json properties
        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string? Ticker { get; set; }

        /// <summary>
        /// Whether or not this response was adjusted for splits.
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool? Adjusted { get; set; }

        /// <summary>
        /// The status of this request's response.
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string? Status { get; set; }

        /// <summary>
        /// The datetime from the <see cref="Timestamp"/> for the start of the aggregate window.
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public DateTime DateTimestamp => DateTime.UnixEpoch.AddMilliseconds(Timestamp);

        /// <summary>
        /// A request id assigned by the server.
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public string? RequestId { get; set; }
        #endregion
    }
}