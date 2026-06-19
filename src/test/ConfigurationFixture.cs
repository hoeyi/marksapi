
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;

namespace ApiClient.Test;

public class ConfigurationFixture : IDisposable
{
    public ConfigurationFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets<ConfigurationFixture>()
            .AddJsonFile("appsettings.json")
            .Build();
        
        Logger = CreateLogger(Configuration);
    }
    
    public Microsoft.Extensions.Logging.ILogger  Logger { get; init; }
    
    public IConfiguration Configuration { get; }

    public void Dispose() => GC.SuppressFinalize(this);
    
    private static Microsoft.Extensions.Logging.ILogger CreateLogger(IConfiguration? configuration = null)
    {
        var logConfig = new LoggerConfiguration();

        if(configuration is not null)
            logConfig.ReadFrom.Configuration(configuration);
        else
            logConfig
                .WriteTo.Console()
                .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day)
                .WriteTo.File(new CompactJsonFormatter(), "*.json", rollingInterval: RollingInterval.Day);

        Log.Logger = logConfig.CreateLogger();

        var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);

        return loggerFactory.CreateLogger(categoryName: $"{nameof(ApiClient)}.Test");
    }
}