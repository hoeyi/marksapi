using System;
using System.Timers;

namespace ApiClient.Services
{
    /// <summary>
    /// Rate-limiting options for binding to clients.
    /// </summary>
    public sealed class RateOptions
    {
        /// <summary>
        /// Gets or sets the limit on API calls per interval.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the length of the interval in microseconds.
        /// </summary>
        public int Interval { get; set; }
    }
}
