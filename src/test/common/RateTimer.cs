using System;
using ApiClient.Services;
using Microsoft.Extensions.Logging;

namespace ApiClient.Test.Unit;

[Trait(nameof(TestAttributeName.Category), "Unit")]
public class RateTimer_Unit : IClassFixture<UnitFixture>
{
    readonly UnitFixture _fixture;

    public RateTimer_Unit(UnitFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _fixture = fixture;
    }
    [Fact]
    public void IncrementCounter_IsRateLimited_Returns_True()
    {
        // Arrange
        // interval should be long enough for test execution
        var rateTimer = new RateTimer(1, 60); 

        // Act
        rateTimer.Increment();
        
        _fixture.Logger.LogDebug("After action: {method} changed state for {@rateTime}.",
            nameof(IncrementCounter_IsRateLimited_Returns_True),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.True(rateTimer.IsRateLimited),
            () => Assert.Equal(1, rateTimer.Counter));
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
            () => Assert.False(rateTimer.IsRateLimited),
            () => Assert.Equal(0, rateTimer.Counter));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_Not_IsRateLimited_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 1);
        
        // Act
        await rateTimer.CheckLimitOrAwaitIntervalResetAsync();
        _fixture.Logger.LogDebug("After action: {method} changed state for {@rateTime}.",
            nameof(AwaitIntervalResetAsync_Not_IsRateLimited_Returns),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.IsRateLimited),
            () => Assert.Equal(0, rateTimer.Counter));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_IsRateLimited_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 5);
        
        _fixture.Logger.LogInformation(
            "Post-Arrange: {method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_IsRateLimited_Returns),
            rateTimer);
        var cts = new CancellationTokenSource();
        cts.CancelAfter(10000);

        // Act
        rateTimer.Increment();
        bool wasRateLimited = rateTimer.IsRateLimited;
        int hadCounter = rateTimer.Counter;
        var timeOut = rateTimer.CheckLimitOrAwaitIntervalResetAsync(ct: cts.Token);

        _fixture.Logger.LogInformation(
            "Post-Act: {method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_IsRateLimited_Returns),
            rateTimer);

        await timeOut;

        // Assert
        Assert.Multiple(
            () => Assert.True(wasRateLimited), // should be true immediately after counter increment
            () => Assert.Equal(1, hadCounter), // should be 1 immediately after counter increment
            () => Assert.False(rateTimer.IsRateLimited), // should be false after waiting
            () => Assert.Equal(0, rateTimer.Counter)); // shoudl reset to zero after waiting
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_CancelRequested_ThrowsCancellationException()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(1));
        var rateTimer = new RateTimer(1, 5);
        rateTimer.Increment();

        _fixture.Logger.LogDebug(
            "{method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_CancelRequested_ThrowsCancellationException),
            rateTimer);

        // TODO: Needs to simulate a long-enough day to trigger after 1 second.
        Task task = rateTimer.CheckLimitOrAwaitIntervalResetAsync(cts.Token);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
    }
}
