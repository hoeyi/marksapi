using System;
using ApiClient.Services;
using Microsoft.Extensions.Logging;

namespace ApiClient.Test.Unit;

[Trait(nameof(TestAttributeName.Category), "Unit")]
public class RateTimer_Unit : IClassFixture<UnitFixture>
{
    readonly UnitFixture _fixture;
    readonly ILogger _logger;
    public RateTimer_Unit(UnitFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
        _logger = Fixture.CreateLogger<RateTimer>(_fixture.Configuration);
    }
    
    [Fact]
    public void InitialState_IsRateLimited_Returns_False()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 60);

        // Act
        // Do nothing
        _fixture.Logger.LogDebug("After action: {method} changed state for {@rateTime}.",
            nameof(InitialState_IsRateLimited_Returns_False),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.IsRateLimited()),
            () => Assert.Equal(0, rateTimer.Counter));
    }

    [Fact]
    public void Increment_IsRateLimited_Returns_True()
    {
        // Arrange
        // interval should be long enough for test execution
        var rateTimer = new RateTimer(1, 60); 

        // Act
        rateTimer.Increment();
        
        _fixture.Logger.LogInformation("After action: {method} changed state for {@rateTimer}.",
            nameof(Increment_IsRateLimited_Returns_True),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.True(rateTimer.IsRateLimited()),
            () => Assert.Equal(1, rateTimer.Counter));
    }

    [Fact]
    public void Increment_ToRateLimit_Invokes_RateLimited()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 60);

        // Act
        // Do nothing

        // Assert
        var evt = Assert.Raises<RateTimer.RateLimitedArgs>(
            h => rateTimer.RateLimited += h,
            h => rateTimer.RateLimited -= h,
            () =>
            {
                rateTimer.Increment();
                rateTimer.EvaluateRateLimit(out _);
            });

        Assert.Multiple(
            () => Assert.NotNull(evt),
            () => Assert.Equal(rateTimer, evt.Sender),
            () => Assert.IsType<RateTimer.RateLimitedArgs>(evt.Arguments)
        );
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_IsRateLimited_Not_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 1);
        var cts = new CancellationTokenSource();
        cts.CancelAfter(3000);

        _fixture.Logger.LogInformation(
            "Post-Arrange: {method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_IsRateLimited_Not_Returns),
            rateTimer);

        // Act
        await rateTimer.CheckLimitOrAwaitIntervalResetAsync();
        _fixture.Logger.LogDebug("After action: {method} changed state for {@rateTime}.",
            nameof(AwaitIntervalResetAsync_IsRateLimited_Not_Returns),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.IsRateLimited()),
            () => Assert.Equal(0, rateTimer.Counter));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_IsRateLimited_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 5, _logger);
        var cts = new CancellationTokenSource();
        cts.CancelAfter(15000);

        _fixture.Logger.LogInformation(
            "Post-Arrange: {method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_IsRateLimited_Returns),
            rateTimer);

        // Act
        rateTimer.Increment();
        bool wasRateLimited = rateTimer.IsRateLimited();
        int hadCounter = rateTimer.Counter;
        await rateTimer.CheckLimitOrAwaitIntervalResetAsync(ct: cts.Token);

        // _fixture.Logger.LogInformation(
        //     "Post-Act: {method} {@rateTime}", 
        //     nameof(AwaitIntervalResetAsync_IsRateLimited_Returns),
        //     rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.True(wasRateLimited), // should be true immediately after counter increment
            () => Assert.Equal(1, hadCounter) // should be 1 immediately after counter increment
            // () => Assert.False(rateTimer.IsRateLimited), // should be false after waiting
            // () => Assert.Equal(0, rateTimer.Counter)); // shoudl reset to zero after waiting
        );
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_CancelRequested_ThrowsCancellationException()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var rateTimer = new RateTimer(1, 5);
        rateTimer.Increment();

        _fixture.Logger.LogDebug(
            "{method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_CancelRequested_ThrowsCancellationException),
            rateTimer);

        // TODO: Needs to simulate a long-enough delay to trigger after 1 second.
        cts.Cancel();
        Task task = rateTimer.CheckLimitOrAwaitIntervalResetAsync(cts.Token);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
    }
}
