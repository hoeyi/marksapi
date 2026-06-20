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
    public void IncrementCounter_RateLimited_Returns_True()
    {
        // Arrange
        // interval should be long enough for test execution
        var rateTimer = new RateTimer(1, 60); 

        // Act
        rateTimer.IncrementCounter();
        
        _fixture.Logger.LogDebug("After action: {method} changed state for {@rateTime}.",
            nameof(IncrementCounter_RateLimited_Returns_True),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.True(rateTimer.RateLimited),
            () => Assert.NotNull(rateTimer.NextReset));
    }
    
    [Fact]
    public void InitialState_RateLimited_Returns_False()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 60);

        // Act
        // Do nothing
        _fixture.Logger.LogDebug("After action: {method} changed state for {@rateTime}.",
            nameof(InitialState_RateLimited_Returns_False),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.RateLimited),
            () => Assert.Null(rateTimer.NextReset));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_Not_RateLimited_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 1);
        
        // Act
        await rateTimer.AwaitIntervalResetAsync();
        _fixture.Logger.LogDebug("After action: {method} changed state for {@rateTime}.",
            nameof(AwaitIntervalResetAsync_Not_RateLimited_Returns),
            rateTimer);

        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.RateLimited),
            () => Assert.Equal(0, rateTimer.Counter));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_RateLimited_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 5);
        
        _fixture.Logger.LogInformation(
            "Post-Arrange: {method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_RateLimited_Returns),
            rateTimer);

        // Act
        rateTimer.IncrementCounter();
        bool rateLimited = rateTimer.RateLimited;
        var timeOut = rateTimer.AwaitIntervalResetAsync(ct: null);

        _fixture.Logger.LogInformation(
            "Post-Act: {method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_RateLimited_Returns),
            rateTimer);

        await timeOut;

        // Assert
        Assert.Multiple(
            () => Assert.True(rateLimited),
            () => Assert.False(rateTimer.RateLimited), // should be false after waiting
            () => Assert.Equal(0, rateTimer.Counter));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_CancelRequested_ThrowsCancellationException()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(1000);
        var rateTimer = new RateTimer(1, 5);
        rateTimer.IncrementCounter();

        _fixture.Logger.LogDebug(
            "{method} {@rateTime}", 
            nameof(AwaitIntervalResetAsync_CancelRequested_ThrowsCancellationException),
            rateTimer);

        // TODO: Needs to simulate a long-enough day to trigger after 1 second.
        Task task = rateTimer.AwaitIntervalResetAsync(cts.Token);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
    }
}
