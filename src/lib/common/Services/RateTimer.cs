using System;
using System.Timers; 

namespace ApiClient.Services;

/// <summary>
/// Provides functionality for counting API calls for client rate-limiting.
/// </summary>
public class RateTimer
{
    private short _counter;
    private readonly Timer _timer;

    /// <summary>
    /// Constructs a new instance of <see cref="RateTimer"/>.
    /// </summary>
    /// <param name="apiCallLimit">The API call limit per interval. Allowable range (0, 1000].</param>
    /// <param name="apiCallInterval">The API interval in seconds. Allowable range (0, 3600)</param>
    public RateTimer(int apiCallLimit, short apiCallInterval)
    {
        // Validate arguments.
        ArgumentOutOfRangeException.ThrowIfLessThan(apiCallLimit, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(apiCallLimit, 1000);

        ArgumentOutOfRangeException.ThrowIfLessThan(apiCallInterval, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(apiCallInterval, 3600);

        ApiCallLimit = apiCallLimit;
        ApiCallInterval = apiCallInterval;

        _timer = new Timer(interval: apiCallInterval * 1000);
        _timer.Elapsed += OnTimerElapsed;
    }

    /// <summary>
    /// Gets the API call limit over <see cref="ApiCallInterval"/> for this timer.
    /// </summary>
    public int ApiCallLimit { get; private set; }

    /// <summary>
    /// Gets the API call interval in seconds for this timer.
    /// </summary>
    public short ApiCallInterval { get; private set; } 

    /// <summary>
    /// Gets the rate-limiting status of this limiter.
    /// </summary>
    public bool RateLimited => _counter == ApiCallLimit;

    /// <summary>
    /// Increments the counter.
    /// </summary>
    public void IncrementCounter() => _counter++;

    /// <summary>
    /// Resets <see cref="_counter"/> to zero.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private void OnTimerElapsed(object? source, ElapsedEventArgs e) => _counter = 0;
}
