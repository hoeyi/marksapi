using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ApiClient.Services;

/// <summary>
/// Provides functionality for counting API calls for client rate-limiting.
/// </summary>
public class RateTimer
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
        ApiCallInterval = apiCallInterval;
        _timer = new(apiCallInterval * 1000)
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
    /// Gets the API call interval in seconds for this timer.
    /// </summary>
    public int ApiCallInterval { get; private init; } 

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
                _logger?.LogInformation("Sleeping for {totalSeconds}", span.TotalSeconds);
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
                            .Where(x => x > timestamp.AddSeconds(ApiCallInterval * -1))
                            .ToList();
        
        if(windowRequests.Count() < ApiCallLimit)
        {
            timeout = null;
            return false;
        }

        // Calculate the next reset (time when the window from the earliest call expires)
        var dt = windowRequests.Min().AddSeconds(ApiCallInterval);
        timeout = dt.Subtract(timestamp);

        Debug.Assert(timeout?.TotalSeconds > 0);
        LogDebug_RateLimited(_logger, windowRequests.Count(), dt);

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
                        .Where(predicate: x => x < signalTime.AddSeconds(ApiCallInterval * -1))
                        .ToArray();
        
        if(expired.Length > 0)
            LogDebug_ExpiredRecords(_logger, expired.Length, expired);

        foreach (var e in expired)
        {
            if(_requestBuffer.TryDequeue(out DateTime result))
                LogDebug_Decrement(_logger, result);
        }
    }

    /// <summary>
    /// Handles the interval time elapsing.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimerElapsed(object? sender, ElapsedEventArgs e) => Decrement(e.SignalTime);

#region Logger methods
    private static void LogDebug_Decrement(
            ILogger? logger, 
            DateTime dateTime)
    {
        if(logger?.IsEnabled(LogLevel.Information) ?? false)
            logger?.LogInformation("{dateTime} successfully dequeued.", dateTime);
    }

    private static void LogDebug_ExpiredRecords(
            ILogger? logger, 
            int count,
            DateTime[] expired)
    {
        if(logger?.IsEnabled(LogLevel.Information) ?? false)
            logger?.LogInformation("Found {count} records to dequeue.\n{@expired}", count, expired);
    }

    private static void LogDebug_RateLimited(
            ILogger? logger, 
            int count,
            DateTime timeOut)
    {
        if(logger?.IsEnabled(LogLevel.Information) ?? false)
            logger?.LogInformation("Rate limited as {count}. Next reset at {tiumeOut}", count, timeOut);
    }
#endregion Logger methods

    public class RateLimitedArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the <see cref="DateTime"/> upon which the next estimated reset will occur.
        /// </summary>
        public DateTime NextReset { get; init; }
    }
}