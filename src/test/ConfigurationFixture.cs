
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ApiClient.Test;

public class ConfigurationFixture : IDisposable
{
    public ConfigurationFixture()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets<ConfigurationFixture>()
            .Build();
        
        Logger = CreateLogger();
    }
    
    public Microsoft.Extensions.Logging.ILogger  Logger { get; init; }
    
    public IConfiguration Configuration { get; }

    public void Dispose() => GC.SuppressFinalize(this);
    
    private static Microsoft.Extensions.Logging.ILogger CreateLogger()
    {

        Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(".log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

        var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);

        return loggerFactory.CreateLogger(categoryName: $"{nameof(ApiClient)}.Test");
    }
}