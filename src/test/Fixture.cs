
using ApiClient.Massive;
using ApiClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace ApiClient.Test;

public class Fixture
{
    public required Microsoft.Extensions.Logging.ILogger  Logger { get; init; }
    
    public required IConfiguration Configuration { get; init; }

    public static Microsoft.Extensions.Logging.ILogger CreateLogger<T>(
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
public class IntegrationFixture : Fixture, IDisposable
{
    public IntegrationFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets<IntegrationFixture>()
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
        
        Logger = CreateLogger<IntegrationFixture>(Configuration);
    }
    
    public MassiveApi MassiveApi { get; init; }

    public void Dispose()
    {
        MassiveApi.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class UnitFixture : Fixture
{
    public UnitFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            
        Logger = CreateLogger<UnitFixture>(Configuration);
    }
}