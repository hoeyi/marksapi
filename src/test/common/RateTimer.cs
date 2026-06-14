using System;
using ApiClient.Services;

namespace ApiClient.Test.Unit;

[Trait(nameof(TestAttributeName.Category), "Unit")]
public class RateTimer_Unit
{
    [Fact]
    public void IncrementCounter_RateLimited_Returns_True()
    {
        // Arrange
        // interval should be long enough for test execution
        var rateTimer = new RateTimer(1, 60); 

        // Act
        rateTimer.IncrementCounter();
        
        // Assert
        Assert.Multiple(
            () => Assert.True(rateTimer.RateLimited),
            () => Assert.True(rateTimer.TimeToReset?.Milliseconds > 0));
    }
    
    [Fact]
    public void InitialState_RateLimited_Returns_False()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 60);

        // Act
        // Do nothing

        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.RateLimited),
            () => Assert.Null(rateTimer.TimeToReset));
    }

    [Fact]
    public void ElapsedTimer_Resets_Counter()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 1);

        // Act
        rateTimer.IncrementCounter();
        int t_zero_count = rateTimer.CurrentIntervalCalls;

        // wait
        Thread.Sleep(2000);
        
        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.RateLimited),
            () => Assert.Equal(0, rateTimer.CurrentIntervalCalls));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_Not_RateLimited_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 1);
        
        // Act
        await rateTimer.AwaitIntervalResetAsync();
        
        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.RateLimited),
            () => Assert.Equal(0, rateTimer.CurrentIntervalCalls));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_RateLimited_Returns()
    {
        // Arrange
        var rateTimer = new RateTimer(1, 5);
        
        // Act
        rateTimer.IncrementCounter();
        await rateTimer.AwaitIntervalResetAsync();
        
        // Assert
        Assert.Multiple(
            () => Assert.False(rateTimer.RateLimited),
            () => Assert.Equal(0, rateTimer.CurrentIntervalCalls));
    }

    [Fact]
    public async Task AwaitIntervalResetAsync_CancelRequested_ThrowsCancellationException()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.CancelAfter(1000);
        var rateTimer = new RateTimer(1, 5);
        rateTimer.IncrementCounter();
        Task task = rateTimer.AwaitIntervalResetAsync(cts.Token);
        
        // Act
        // Assert
        await Assert.ThrowsAsync<OperationCanceledException>(async () => await task);
    }
}
