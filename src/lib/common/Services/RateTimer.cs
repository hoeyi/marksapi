using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ApiClient.Services;

/// <summary>
/// Provides functionality for counting API calls for client rate-limiting.
/// </summary>
public sealed class RateTimer
{
    private short _counter;
    private DateTime? _nextReset;
    private readonly System.Timers.Timer _timer;
    private readonly object _lock = new();
    private readonly ConcurrentQueue<DateTime> _requestBuffer = [];

    /// <summary>
    /// Constructs a new instance of <see cref="RateTimer"/>.
    /// </summary>
    /// <param name="apiCallLimit">The API call limit per interval. Allowable range (0, 1000].</param>
    /// <param name="apiCallInterval">The API interval in seconds. Allowable range (0, 3600)</param>
    public RateTimer(int apiCallLimit, int apiCallInterval)
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
    }

    private void TimerElapsed(object? sender, ElapsedEventArgs e) => ResetCounter();

    public event EventHandler<RateLimitedArgs>? RateLimited;

    /// <summary>
    /// Gets the API call limit over <see cref="ApiCallInterval"/> for this timer.
    /// </summary>
    public int ApiCallLimit { get; private init; }

    /// <summary>
    /// Gets the API call interval in seconds for this timer.
    /// </summary>
    public int ApiCallInterval { get; private init; } 

    private float EnforcedRate => ApiCallLimit / ApiCallInterval;

    /// <summary>
    /// Gets the rate-limiting status of this limiter.
    /// </summary>
    // public bool IsRateLimited => Counter >= ApiCallLimit && NextReset > DateTime.UtcNow;
    internal bool IsRateLimited
    {
        get
        {
            lock (_lock)
            {
                return _counter >= ApiCallLimit && _nextReset > DateTime.UtcNow;
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="DateTime"/> representing the next estimated reset.
    /// </summary>
    internal DateTime? NextReset => _nextReset;

    /// <summary>
    /// Gets or sets the count of API calls in this interval.
    /// </summary>
    internal short Counter => _counter;

    /// <summary>
    /// Increments the <see cref="Counter"> property.
    /// </summary>
    public void IncrementCounter()
    {
        _requestBuffer.Enqueue(DateTime.UtcNow);
        if(_requestBuffer.Count >= ApiCallLimit)
        {
            _nextReset = DateTime.UtcNow.AddSeconds(ApiCallInterval);
            var eventArgs = new RateLimitedArgs()
            {
                NextReset = (DateTime)_nextReset
            };
            RateLimited?.Invoke(sender: this, e: eventArgs);
        }
    }

    /// <summary>
    /// Checks to see if the rate limit has been tripped and awaits until the next reset before 
    /// returning.
    /// </summary>
    /// <param name="ct">A <see cref="CancellationToken"/> instance.</param>
    /// <returns>An empty <see cref="Task"/>.</returns>
    public async Task CheckLimitOrAwaitIntervalResetAsync(CancellationToken? ct = null)
    {
        while(IsRateLimited)
        {
            while(!(ct?.IsCancellationRequested ?? false))
            {
                
                TimeSpan timeOut = _nextReset?.Subtract(DateTime.UtcNow) ?? new();

                if(timeOut.Seconds > 0)
                    await Task.Delay(timeOut);
                else
                    break;
                // ResetCounter();
            }
            ct?.ThrowIfCancellationRequested();
        }

        return;
    }

    /// <summary>
    /// Resets the <see cref="_counter"/> and <see cref="_nextReset"/> fields.
    /// </summary>
    private void ResetCounter()
    {
        lock (_lock)
        {
            _counter = 0;
            _nextReset = null;
        }
    }

    private void Dequeue()
    {
        var currentTime = DateTime.UtcNow;
        var outsideWindowDateTimes = _requestBuffer.Select(x => x.AddSeconds(ApiCallInterval) < currentTime);
        foreach(var dt in outsideWindowDateTimes)
            _requestBuffer.De(dt, out bool _);
    }
    private bool EvaluateRateLimit(out TimeSpan? timeout)
    {
        var currentCount = _requestBuffer.Count();
        if(currentCount == 0)
        {
            timeout = null;
            return false;
        }

        // Clean-up events outside the window
        var dt = _requestBuffer.First();
        var timestamp = DateTime.UtcNow;
        while(dt.AddSeconds(ApiCallInterval) < timestamp)
        {
            if(_requestBuffer.TryDequeue(out DateTime result))
                dt = result;
            else
                break;

        }
        
    }
}

public class RateLimitedArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the <see cref="DateTime"/> upon which the next estimated reset will occur.
    /// </summary>
    public DateTime NextReset { get; init; }
}