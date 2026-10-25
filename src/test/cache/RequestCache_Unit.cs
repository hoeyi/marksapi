using ApiClient.Cache;
using ApiClient.Cache.Entity;
using Microsoft.Extensions.Configuration;
using System;

namespace ApiClient.Test.Cache;

public class RequestCacheFixture : UnitFixture<RequestCache>
{
    protected readonly RequestDbContext _dbContext;

    public RequestCacheFixture() : base()
    {
        ArgumentNullException.ThrowIfNull(Configuration);
        _dbContext = new RequestDbContext(Configuration);
    }

    protected override IConfiguration GetConfiguration() => 
                            new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .AddJsonFile("appsettings.cache.json")
                                .Build();
}

/// <summary>
/// Handles interaction with the SQLite data store as an in-memory database.
/// </summary>
public sealed class RequestCache_Unit_InMemory : RequestCache_UnitTest_Contract
{
    public RequestCache_Unit_InMemory(RequestCacheFixture fixture) : base(fixture)
    {
    }

    protected override IRequestCache<string> CreateCache() => 
        new RequestCache(_unitFixture.Configuration);
}

public abstract class RequestCache_UnitTest_Contract : IClassFixture<RequestCacheFixture>
{
    protected readonly RequestCacheFixture _unitFixture;

    public RequestCache_UnitTest_Contract(RequestCacheFixture unitFixture)
    {
        ArgumentNullException.ThrowIfNull(unitFixture);
        _unitFixture = unitFixture;
    }

    protected abstract IRequestCache<string> CreateCache();

    protected static CancellationToken Ct => CancellationToken.None;


    // A miss is a faulted task, not a cancelled one.
    protected static async Task AssertMissAsync<T>(Task<T> task)
    {
        await Record.ExceptionAsync(() => task);

        Assert.True(task.IsFaulted);
    }

    [Fact]
    public async Task ContainsAsync_ReturnsFalse_WhenCacheIsEmpty()
    {
        // Arrange
        var cache = CreateCache();
        
        // Act
        // Assert
        Assert.False(await cache.ContainsAsync(TestRequests.Create("a"), Ct));
    }

    [Fact]
    public async Task ContainsAsync_ReturnsTrue_AfterSet()
    {
        // Arrange
        var cache = CreateCache();
        var request = TestRequests.Create("a");
        
         // Act
        await cache.SetAsync(request, "value", Ct);
        
         // Assert
        Assert.True(await cache.ContainsAsync(request, Ct));
    }

    [Fact]
    public async Task GetAsync_ReturnsFaultedTask_OnMiss()
    {
        // Arrange
        var cache = CreateCache();
        
         // Act
         var result = cache.GetAsync(TestRequests.Create("a"), Ct);
         await result;

         // Assert
        Assert.True(result.IsFaulted);
    }

    [Fact]
    public async Task GetAsync_ReturnsStoredResult_AfterSet()
    {
        // Arrange
        var cache = CreateCache();
        var request = TestRequests.Create("a");
        var expected = "value";
        
        // Act
        await cache.SetAsync(request, expected, Ct);
        
        // Assert
        Assert.Equal(expected, await cache.GetAsync(request, Ct));
    }

    [Fact]
    public async Task GetAsync_MatchesEquivalentRequestInstance()
    {
        // Arrange
        var cache = CreateCache();
        var expected = "value";
        
        // Act
        await cache.SetAsync(TestRequests.Create("a"), expected, Ct);
        
        // Assert
        Assert.Equal(expected, await cache.GetAsync(TestRequests.Create("a"), Ct));
    }

    [Fact]
    public async Task GetAsync_DoesNotReturnEntryForDifferentRequest()
    {
        var cache = CreateCache();

        await cache.SetAsync(TestRequests.Create("a"), "value", Ct);

        Assert.False(await cache.ContainsAsync(TestRequests.Create("b"), Ct));
        await AssertMissAsync(cache.GetAsync(TestRequests.Create("b"), Ct));
    }

    [Fact]
    public async Task SetAsync_OverwritesExistingEntry()
    {
        // Arrange
        var cache = CreateCache();
        var request = TestRequests.Create("a");
        
        //Act
        await cache.SetAsync(request, "first", Ct);
        await cache.SetAsync(request, "second", Ct);
        
        // Assert
        Assert.Equal("second", await cache.GetAsync(request, Ct));
    }

    [Fact]
    public async Task SetAsync_OverwriteDoesNotAffectOtherEntries()
    {
        // Arrange
        var cache = CreateCache();
        
        //Act
        await cache.SetAsync(TestRequests.Create("a"), "a1", Ct);
        await cache.SetAsync(TestRequests.Create("b"), "b1", Ct);
        await cache.SetAsync(TestRequests.Create("a"), "a2", Ct);
        
        // Assert
        Assert.Equal("b1", await cache.GetAsync(TestRequests.Create("b"), Ct));
    }

    [Fact]
    public async Task FlushAsync_RemovesEveryEntry()
    {
        // Arrange
        var cache = CreateCache();
        
        //Act
        await cache.SetAsync(TestRequests.Create("a"), "a", Ct);
        await cache.SetAsync(TestRequests.Create("b"), "b", Ct);
        await cache.FlushAsync(Ct);
        
        // Assert
        Assert.False(await cache.ContainsAsync(TestRequests.Create("a"), Ct));
        Assert.False(await cache.ContainsAsync(TestRequests.Create("b"), Ct));
        await AssertMissAsync(cache.GetAsync(TestRequests.Create("a"), Ct));
        await AssertMissAsync(cache.GetAsync(TestRequests.Create("b"), Ct));
    }

    [Fact]
    public async Task FlushAsync_OnEmptyCache_DoesNotThrow()
    {
        // Arrange
        var cache = CreateCache();
        
        //Act
        var exception = await Record.ExceptionAsync(() => cache.FlushAsync(Ct));
        
        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task SetAsync_AfterFlush_StoresNewEntry()
    {
        // Arrange
        var cache = CreateCache();
        var request = TestRequests.Create("a");
        
        //Act
        await cache.SetAsync(request, "old", Ct);
        await cache.FlushAsync(Ct);

        await cache.SetAsync(request, "new", Ct);
        
        // Assert
        Assert.Equal("new", await cache.GetAsync(request, Ct));
    }

    // Inferred: operations honor an already-cancelled token.
    [Theory]
    [InlineData("Contains")]
    [InlineData("Get")]
    [InlineData("Set")]
    [InlineData("Flush")]
    public async Task Operations_Throw_WhenTokenIsCancelled(string operation)
    {
        // Arrange
        var cache = CreateCache();
        var request = TestRequests.Create("a");
        var cancelled = new CancellationToken(canceled: true);

        Func<Task> act = operation switch
        {
            "Contains" => () => cache.ContainsAsync(request, cancelled),
            "Get" => () => cache.GetAsync(request, cancelled),
            "Set" => () => cache.SetAsync(request, "value", cancelled),
            "Flush" => () => cache.FlushAsync(cancelled),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
        
        //Act
        // Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(act);
    }

    // Inferred: null arguments are rejected.
    [Theory]
    [InlineData("Contains")]
    [InlineData("Get")]
    [InlineData("SetRequest")]
    [InlineData("SetResult")]
    public async Task Operations_Throw_OnNullArguments(string operation)
    {
        // Arrange
        var cache = CreateCache();

        Func<Task> act = operation switch
        {
            "Contains" => () => cache.ContainsAsync(null!, Ct),
            "Get" => () => cache.GetAsync(null!, Ct),
            "SetRequest" => () => cache.SetAsync(null!, "value", Ct),
            "SetResult" => () => cache.SetAsync(TestRequests.Create("a"), null!, Ct),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
        
        //Act
        // Assert
        await Assert.ThrowsAsync<ArgumentNullException>(act);
    }
}