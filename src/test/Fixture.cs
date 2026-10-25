
using ApiClient.Massive;
using ApiClient.Services;
using Ichyd.Marksapi.Cli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ApiClient.Test
{
    #region Fixture
    public class Fixture
    {
        public Fixture()
        {
            Configuration = GetConfiguration();
            Logger = CreateLogger<Fixture>(Configuration);

            ArgumentNullException.ThrowIfNull(Configuration);
            ArgumentNullException.ThrowIfNull(Logger);
        }

        public required ILogger Logger { get; init; }
    
        public required IConfiguration Configuration { get; init; }

        protected virtual IConfiguration GetConfiguration() => 
                            new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json")
                                .Build();
        protected static ILogger CreateLogger<T>(
            IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            var logConfig = new LoggerConfiguration();

            if(configuration is not null)
                logConfig.ReadFrom.Configuration(configuration);
            else
                logConfig
                    .WriteTo.Console()
                    .WriteTo.File("logs/.log", rollingInterval: RollingInterval.Day)
                    .WriteTo.File(new CompactJsonFormatter(), "logs/.json", rollingInterval: RollingInterval.Day);

            Log.Logger = logConfig.CreateLogger();

            var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);

            return loggerFactory.CreateLogger<T>();
        }
    }
    #endregion

    #region Integration fixture
    public abstract class IntegrationFixture<T> : Fixture, IDisposable
        where T : IDisposable
    {
        public IntegrationFixture() : base()
        {
            LogInitialize(Logger, typeof(T).FullName!);

            TestInstance = CreateTestInstance();
        }

        /// <summary>
        /// Represents the <typeparamref name="T"/> instance shared across integration 
        /// tests. For a fresh instance from the same configuration, see <see cref="CreateTestInstance"/>.
        /// </summary>
        public T TestInstance { get; init; }

        /// <inheritdoc/>
        public void Dispose()
        {
            LogStartCleanup(Logger, typeof(T).FullName!);

            TestInstance.Dispose();
            LogCleanup(Logger, nameof(TestInstance));
            LogComplete(Logger);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a fresh <typeparamref name="T"/> instance from the <see cref="IConfiguration"/> and 
        /// <see cref="ILogger"/> members.
        /// </summary>
        /// <returns>An <typeparamref name="T"/>./returns>
        public abstract T CreateTestInstance();

        #region Logger methods
        private static void LogInitialize(ILogger? logger, string @class)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger.LogInformation(
                    "Initialzed integration test context for {class}.",
                    @class);
        }

        private static void LogStartCleanup(ILogger logger, string @class)
        {
            if(logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Cleaning integration test context for {class}", @class);
        }

        private static void LogCleanup(ILogger logger, string member)
        {
            if(logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Cleaned up {member}", member);
        }

        private static void LogComplete(ILogger logger)
        {
            if(logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Done.");
        }
        #endregion
    }
    #endregion

    #region Unit fixture
    public class UnitFixture<T> : Fixture
    {
        public UnitFixture() : base()
        {
            LogInitialize(Logger, typeof(T).FullName!);
        }

        private static void LogInitialize(ILogger? logger, string @class)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger.LogInformation(
                    "Initialzed unit test context for {class}.",
                    @class);
        }
    }
    #endregion
}