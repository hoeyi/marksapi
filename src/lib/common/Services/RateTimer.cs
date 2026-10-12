using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ApiClient.Services
{
    /// <summary>
    /// Provides functionality for counting API calls for client rate-limiting.
    /// </summary>
    public class RateTimer : IDisposable
    {
        private readonly System.Timers.Timer _timer;
        private readonly ConcurrentQueue<DateTime> _requestBuffer = [];

        private readonly ILogger? _logger;
        /// <summary>
        /// Constructs a new instance of <see cref="RateTimer"/>.
        /// </summary>
        /// <param name="apiCallLimit">The API call limit per interval. Allowable range (0, 1000].</param>
        /// <param name="apiCallInterval">The API interval in seconds. Allowable range (0, 3600)</param>
        public RateTimer(int apiCallLimit, int apiCallInterval, ILogger? logger = null)
        {
            // Validate arguments.
            ArgumentOutOfRangeException.ThrowIfLessThan(apiCallLimit, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(apiCallLimit, 1000);

            ArgumentOutOfRangeException.ThrowIfLessThan(apiCallInterval, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(apiCallInterval, 3600);

            ApiCallLimit = apiCallLimit;
            ApiCallInterval = TimeSpan.FromSeconds(apiCallInterval);
            _timer = new(ApiCallInterval)
            {
                AutoReset = true,
                Enabled = true
            };
            _timer.Elapsed += TimerElapsed;
            _logger = logger;
        }
    
        /// <summary>
        /// Event raised when the rate limit is tripped.
        /// </summary>
        public event EventHandler<RateLimitedArgs>? RateLimited;

        /// <summary>
        /// Gets the API call limit over <see cref="ApiCallInterval"/> for this timer.
        /// </summary>
        public int ApiCallLimit { get; private init; }

        /// <summary>
        /// Gets the API call interval for this timer.
        /// </summary>
        public TimeSpan ApiCallInterval { get; private init; }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }
                // Release unmanaged resources.
                // Set large fields to null.                
                disposed = true;
            }
        }

        public void Dispose()
        {
            _timer.Elapsed -= TimerElapsed;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the rate-limiting status of this limiter.
        /// </summary>
        // public bool IsRateLimited => Counter >= ApiCallLimit && NextReset > DateTime.UtcNow;
        public bool IsRateLimited() => EvaluateRateLimit(out _);

        /// <summary>
        /// Gets or sets the count of API calls in this interval.
        /// </summary>
        public int Counter => _requestBuffer.Count();

        /// <summary>
        /// Checks to see if the rate limit has been tripped and awaits until the next reset before 
        /// returning.
        /// </summary>
        /// <param name="ct">A <see cref="CancellationToken"/> instance.</param>
        /// <returns>An empty <see cref="Task"/>.</returns>
        public async Task CheckLimitOrAwaitIntervalResetAsync(CancellationToken? ct = null)
        {
            TimeSpan? timeout = null;
            while(EvaluateRateLimit(out timeout) && timeout is TimeSpan span)
            {
                ct?.ThrowIfCancellationRequested();

                if(span.TotalSeconds > 0)
                {
                    await Task.Delay(delay: span, cancellationToken: ct ?? default);
                }
                else
                    break;
            }

            return;
        }

        /// <summary>
        /// Checks the request buffer to determine if rate limiting applies.
        /// </summary>
        /// <param name="timeout">If rate limited, the delay until the first in item in the queue expires.</param>
        /// <returns><see cref="true"/>if rate limited, else false.</returns>
        public bool EvaluateRateLimit(out TimeSpan? timeout)
        {
            var timestamp = DateTime.UtcNow;
            var windowRequests = _requestBuffer
                                .Where(x => x > timestamp.AddSeconds(ApiCallInterval.TotalSeconds * -1))
                                .ToList();
        
            if(windowRequests.Count < ApiCallLimit)
            {
                timeout = null;
                return false;
            }

            // Calculate the next reset (time when the window from the earliest call expires)
            var dt = windowRequests.Min().AddSeconds(ApiCallInterval.TotalSeconds);
            timeout = dt.Subtract(timestamp);

            RateLimited?.Invoke(this, new(){ NextReset = dt});
            return true;
        }

        /// <summary>
        /// Increments the rate counter.
        /// </summary>
        /// <returns>An <see cref="int"/> representing the number of queued items.</returns>
        public int Increment()
        {
            _requestBuffer.Enqueue(DateTime.UtcNow);
            return _requestBuffer.Count();
        }
    
        /// <summary>
        /// Handles clean-up of <see cref="_requestBuffer"/> by clearing any items in the 
        /// queue outside the API call window, based on the given time.
        /// </summary>
        /// <param name="signalTime">The time for the end of the window.</param>
        private void Decrement(DateTime signalTime)
        {
            var expired = _requestBuffer
                            .Where(predicate: 
                                x => x < signalTime.AddSeconds(ApiCallInterval.TotalSeconds * -1))
                            .ToArray();
        
            if(expired.Length > 0)
                LogDebug_RecordsFoundForDequeue(_logger, expired.Length, expired);

            foreach (var e in expired)
            {
                if(_requestBuffer.TryDequeue(out DateTime result))
                    LogDebug_RecordDequeue_Success(_logger, result);
            }
        }

        /// <summary>
        /// Handles the interval time elapsing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object? sender, ElapsedEventArgs e) => Decrement(e.SignalTime);

        #region Logger methods
        private static void LogDebug_RecordDequeue_Success(
                ILogger? logger, 
                DateTime dateTime)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger?.LogInformation("Request '{dateTime}' successfully dequeued.", dateTime);
        }

        private static void LogDebug_RecordsFoundForDequeue(
                ILogger? logger, 
                int count,
                DateTime[] expired)
        {
            if(logger?.IsEnabled(LogLevel.Debug) ?? false)
                logger?.LogDebug("Found {count} records to dequeue.\n{@expired}", count, expired);
        }
        #endregion Logger methods

        public class RateLimitedArgs : EventArgs
        {
            /// <summary>
            /// Gets or sets the timeout used to estimate the <see cref="NextReset"/>.
            /// </summary>
            public TimeSpan TimeOut { get; init; }

            /// <summary>
            /// Gets or sets the <see cref="DateTime"/> upon which the next estimated reset will occur.
            /// </summary>
            public DateTime NextReset { get; init; }
        }
    }

}