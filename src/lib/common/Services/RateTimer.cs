using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Timers; 

namespace ApiClient.Services;

/// <summary>
/// Provides functionality for counting API calls for client rate-limiting.
/// </summary>
public struct RateTimer
{
    private short _counter;
    private DateTime? _lastReset;
    private readonly object _lock = new();
    private readonly System.Timers.Timer _timer;

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

        _timer = new System.Timers.Timer(interval: apiCallInterval * 1000);
        _timer.Elapsed += OnTimerElapsed;
        _timer.Enabled = true;
    }

    /// <summary>
    /// Gets the API call limit over <see cref="ApiCallInterval"/> for this timer.
    /// </summary>
    public int ApiCallLimit { get; private init; }

    /// <summary>
    /// Gets the API call interval in seconds for this timer.
    /// </summary>
    public int ApiCallInterval { get; private init; } 

    /// <summary>
    /// Gets the count of calls in the current interval.
    /// </summary>
    public short CurrentIntervalCalls => Counter;

    /// <summary>
    /// Gets the rate-limiting status of this limiter.
    /// </summary>
    public bool RateLimited => Counter == ApiCallLimit;
 
    /// <summary>
    /// Gets the <see cref="TimeSpan"/> representing the difference between now 
    /// and the <see cref="LastReset"/> timestamp.
    /// </summary>
    public TimeSpan? TimeToReset => LastReset?.AddSeconds(ApiCallInterval).Subtract(DateTime.UtcNow);

    /// <summary>
    /// Gets or sets the count of API calls in this interval.
    /// </summary>
    private short Counter
    {
        get { return _counter; }
        set
        {
            lock (_lock)
            {
                if(_counter != value)
                {
                    _counter = value;
                    _lastReset = DateTime.UtcNow;
                }
            }
        }
    }
    
    /// <summary>
    /// Gets the UTC timestamp for the last time the <see cref="Counter"/> value was changed.
    /// </summary>
    private DateTime? LastReset => _lastReset;

    /// <summary>
    /// Increments the <see cref="Counter"> property.
    /// </summary>
    public void IncrementCounter() => Counter++;

    /// <summary>
    /// Checks to see if the rate limit has been tripped and awaits until the next reset before 
    /// returning.
    /// </summary>
    /// <param name="ct">A <see cref="CancellationToken"/> instance.</param>
    /// <returns>An empty <see cref="Task"/>.</returns>
    public async Task AwaitIntervalResetAsync(CancellationToken? ct = null)
    {
        ct?.ThrowIfCancellationRequested();

        // Validate parameters.
        if(!RateLimited)
            return;
        
        if(TimeToReset is null)
            throw new InvalidOperationException(
                $"Expected non-null value for '{nameof(TimeToReset)}'.");

        while(Counter > 0)
        {
            if(ct?.IsCancellationRequested ?? false)
            {
                // Clean up here.
                ct?.ThrowIfCancellationRequested();
            }
            Thread.Sleep(
                timeout: TimeToReset ?? 
                         new TimeSpan(hours: 0, minutes: 0, seconds: 0));
        }

        return;
    }
    
    /// <summary>
    /// Resets <see cref="Counter"/> to zero.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private void OnTimerElapsed(object? source, ElapsedEventArgs e)
    {
        Counter = 0;
    }
}
