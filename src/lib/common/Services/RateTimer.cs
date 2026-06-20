using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiClient.Services;

/// <summary>
/// Provides functionality for counting API calls for client rate-limiting.
/// </summary>
public class RateTimer
{
    private short _counter;
    private DateTime? _nextReset;

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
    /// Gets the rate-limiting status of this limiter.
    /// </summary>
    public bool RateLimited => Counter >= ApiCallLimit && NextReset > DateTime.UtcNow;
 
    /// <summary>
    /// Gets the <see cref="DateTime"/> representing the next estimated reset.
    /// </summary>
    public DateTime? NextReset => _nextReset;

    /// <summary>
    /// Gets or sets the count of API calls in this interval.
    /// </summary>
    public short Counter => _counter;

    /// <summary>
    /// Increments the <see cref="Counter"> property.
    /// </summary>
    public void IncrementCounter()
    {
        _counter++;
        if(_counter >= ApiCallLimit)
            _nextReset = DateTime.UtcNow.AddSeconds(ApiCallInterval);
    }

    /// <summary>
    /// Checks to see if the rate limit has been tripped and awaits until the next reset before 
    /// returning.
    /// </summary>
    /// <param name="ct">A <see cref="CancellationToken"/> instance.</param>
    /// <returns>An empty <see cref="Task"/>.</returns>
    public async Task AwaitIntervalResetAsync(CancellationToken? ct = null)
    {
        while(RateLimited)
        {
            ct?.ThrowIfCancellationRequested();
            TimeSpan timeOut = NextReset!.Value.Subtract(DateTime.UtcNow);

            await Task.Delay(timeOut);
            ResetCounter();
        }

        return;
    }

    /// <summary>
    /// Resets the <see cref="_counter"/> and <see cref="_nextReset"/> fields.
    /// </summary>
    private void ResetCounter()
    {
        _counter = 0;
        _nextReset = null;
    }
}
