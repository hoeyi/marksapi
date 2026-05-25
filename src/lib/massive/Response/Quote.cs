using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiClient.Massive.Response
{
    /// <summary>
    /// Represents the quote data for a security.
    /// </summary>
    public class Quote
    {
        /// <summary>
        /// The ask price.
        /// </summary>
        [JsonPropertyName("P")]
        public required decimal AskPrice { get; set; }

        /// <summary>
        /// The total number of shares available for sale at the current ask price.
        /// </summary>
        [JsonPropertyName("S")]
        public required int AskSize { get; set; }

        /// <summary>
        /// The exchange symbol that this item is traded under.
        /// </summary>
        [JsonPropertyName("T")]
        public required string Ticker { get; set; }

        /// <summary>
        /// The exchange ID. See Exchanges for Massive's mapping of exchange IDs.
        /// </summary>
        [JsonPropertyName("X")]
        public required int AskExchangeId { get; set; }

        /// <summary>
        /// A list of condition codes.
        /// </summary>
        [JsonPropertyName("c")]
        public required List<int> ConditionCodes { get; set; }

        /// <summary>
        /// The nanosecond accuracy TRF(Trade Reporting Facility) Unix Timestamp. This is the timestamp of when the trade reporting facility received this message.
        /// </summary>
        [JsonPropertyName("f")]
        public required long TrfTimestamp { get; set; }

        /// <summary>
        /// A list of indicator codes.
        /// </summary>
        [JsonPropertyName("i")]
        public required List<int> IndicatorCodes { get; set; }

        /// <summary>
        /// The bid price.
        /// </summary>
        [JsonPropertyName("p")]
        public required decimal BidPrice { get; set; }

        /// <summary>
        /// The sequence number represents the sequence in which message events happened. These are increasing and unique per ticker symbol, but will not always be sequential (e.g., 1, 2, 6, 9, 10, 11).
        /// </summary>
        [JsonPropertyName("q")]
        public required long SequenceNumber { get; set; }

        /// <summary>
        /// The total number of shares that buyers want to purchase at the current bid price.
        /// </summary>
        [JsonPropertyName("s")]
        public required int BidSize { get; set; }

        /// <summary>
        /// The nanosecond accuracy SIP Unix Timestamp. This is the timestamp of when the SIP received this message from the exchange which produced it.
        /// </summary>
        [JsonPropertyName("t")]
        public required long SipTimestamp { get; set; }

        /// <summary>
        /// The exchange ID. See Exchanges for Massive's mapping of exchange IDs.
        /// </summary>
        [JsonPropertyName("x")]
        public required int BidExchangeId { get; set; }

        /// <summary>
        /// The nanosecond accuracy Participant/Exchange Unix Timestamp. This is the timestamp of when the quote was actually generated at the exchange.
        /// </summary>
        [JsonPropertyName("y")]
        public required long ParticipantTimestamp { get; set; }

        /// <summary>
        /// There are 3 tapes which define which exchange the ticker is listed on. These are integers in our objects which represent the letter of the alphabet. Eg: 1 = A, 2 = B, 3 = C. * Tape A is NYSE listed securities * Tape B is NYSE ARCA / NYSE American * Tape C is NASDAQ
        /// </summary>
        [JsonPropertyName("z")]
        public required int Tape { get; set; }
    }
}