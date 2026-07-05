
using ApiClient.Massive;
using ApiClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ApiClient.Test;

public class Fixture
{
    public Fixture()
    {
        Configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
        Logger = CreateLogger<Fixture>(Configuration);
    }
    public required ILogger Logger { get; init; }
    
    public required IConfiguration Configuration { get; init; }

    public static ILogger CreateLogger<T>(
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
public class IntegrationFixture<T> : Fixture, IDisposable
{
    public IntegrationFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationFixture<T>>()
            .AddJsonFile("appsettings.json")
            .Build();
        
        RateOptions options = new();
            var section = Configuration
                .GetSection("massive")?
                .GetSection(nameof(RateOptions));

            if (section is null)
            {
                options.Limit = 5;
                options.Interval = 60;
            }
            else
                section.Bind(options);

        ArgumentException.ThrowIfNullOrWhiteSpace(Configuration?["massive:api_key"]);
        MassiveApi = new MassiveApi(Configuration["massive:api_key"]!, rateOptions: options);
        
        Logger = CreateLogger<T>(Configuration);
        LogInitialize(Logger, typeof(T).FullName!);
    }
    
    public MassiveApi MassiveApi { get; init; }

    public void Dispose()
    {
        LogStartCleanup(Logger, typeof(MassiveApi).FullName!);

        MassiveApi.Dispose();
        LogCleanup(Logger, nameof(MassiveApi));
        LogComplete(Logger);

        GC.SuppressFinalize(this);
    }

    private static void LogInitialize(ILogger logger, string @class)
    {
        if(logger.IsEnabled(LogLevel.Information))
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
}

public class UnitFixture<T> : Fixture
{
    public UnitFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            
        Logger = CreateLogger<T>(Configuration);

        LogInitialize(Logger, typeof(T).FullName!);
    }

    private static void LogInitialize(ILogger logger, string @class)
    {
        if(logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(
                "Initialzed unit test context for {class}.",
                @class);
    }
}