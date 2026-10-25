using ApiClient.Cache;
using ApiClient.Cache.Entity;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace ApiClient.Test.Cache
{
    public class RequestCacheFixture : UnitFixture<RequestCache>, IDisposable
    {
        // protected readonly RequestDbContext _dbContext;
        private readonly SqliteConnection _connection;
        public RequestCacheFixture() : base()
        {
            ArgumentNullException.ThrowIfNull(Configuration);
            // _dbContext = new RequestDbContext(Configuration);
            _connection = new SqliteConnection(Configuration.GetConnectionString("SQLite"));
            _connection.Open();
        }

        protected override IConfiguration GetConfiguration() => 
                                new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")
                                    .AddJsonFile("appsettings.cache.json")
                                    .Build();

        public void Dispose()
        {
            _connection.Close();

            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Handles interaction with the SQLite data store as an in-memory database.
    /// </summary>
    public sealed class RequestCache_Unit_InMemory : RequestCache_UnitTest_Contract
    {
        public RequestCache_Unit_InMemory(RequestCacheFixture fixture) : base(fixture)
        {
        }

        protected override IRequestCache CreateCache() => 
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

        protected abstract IRequestCache CreateCache();

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
            int count = await cache.SetAsync(request, Ct);
        
             // Assert
            Assert.Equal(expected: 1, actual: count);
            Assert.True(await cache.ContainsAsync(request, Ct));
        }

        [Fact]
        public async Task GetAsync_OnMiss_Throws_InvalidOperation()
        {
            // Arrange
            var cache = CreateCache();
            var request = TestRequests.Create("a");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => cache.GetAsync(request.Id, Ct));
        }

        [Fact]
        public async Task GetAsync_AfterSet_ReturnsInstance()
        {
            // Arrange
            var cache = CreateCache();
            var request = TestRequests.Create("a");
        
            // Act
            await cache.SetAsync(request, Ct);
            var result = await cache.GetAsync(request.Id, Ct);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(request.Id, result.Id),
                () => Assert.Equal(request.CreatedAt, result.CreatedAt),
                () => Assert.Equal(request.Command, result.Command)
            );
        }

        [Fact]
        public async Task SetAsync_AfterPriorSet_OverwritesExistingEntry()
        {
            // Arrange
            var cache = CreateCache();
            var request = TestRequests.Create("first");
        
        
            //Act
            await cache.SetAsync(request, Ct);
            request.Command = "second";
            await cache.SetAsync(request, Ct);
        
            var result = await cache.GetAsync(request.Id, Ct);

            // Assert
            Assert.Equal(request.Id, result.Id);
            Assert.Equal("second", result.Command);
        }

        [Fact]
        public async Task SetAsync_AfterSet_OtherRecordsUnchanged()
        {
            // Arrange
            var cache = CreateCache();
            var request_immutable = TestRequests.Create("a");
            var request_mutatable = TestRequests.Create("b");

            await cache.SetAsync(request_immutable, Ct);
            await cache.SetAsync(request_mutatable, Ct);

            //Act
            request_mutatable.Command = "c";
            await cache.SetAsync(request_mutatable, Ct);
        
            var result_mutable = await cache.GetAsync(request_mutatable.Id, Ct);
            var result_immuatable = await cache.GetAsync(request_immutable.Id, Ct);

            // Assert
            Assert.Equal(request_mutatable.Id, result_mutable.Id);
            Assert.Equal("c", result_mutable.Command);
            Assert.Equal(request_immutable.Id, result_immuatable.Id);
            Assert.Equal("a", result_immuatable.Command);
        }

        [Fact]
        public async Task FlushAsync_RemovesEveryEntry()
        {
            // Arrange
            var cache = CreateCache();
        
            //Act
            await cache.SetAsync(TestRequests.Create("a"), Ct);
            await cache.SetAsync(TestRequests.Create("b"), Ct);
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
            var request = TestRequests.Create("old");
            var request_new = TestRequests.Create("new");
        
            //Act
            await cache.SetAsync(request, Ct);
            await cache.FlushAsync(Ct);

            await cache.SetAsync(request_new, Ct);
            var result = await cache.GetAsync(request_new.Id, Ct);
        
            // Assert
            Assert.Equal(request_new.Id, result.Id);
            Assert.Equal("new", result.Command);
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
                "Get" => () => cache.GetAsync(request.Id, cancelled),
                "Set" => () => cache.SetAsync(request, cancelled),
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
        [InlineData("Set")]
        public async Task Operations_Throw_OnNullArguments(string operation)
        {
            // Arrange
            var cache = CreateCache();

            Func<Task> act = operation switch
            {
                "Contains" => () => cache.ContainsAsync(null!, Ct),
                "Get" => () => cache.GetAsync(null!, Ct),
                "Set" => () => cache.SetAsync(null!, Ct),
                _ => throw new ArgumentOutOfRangeException(nameof(operation)),
            };
        
            //Act
            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(act);
        }
    }

}