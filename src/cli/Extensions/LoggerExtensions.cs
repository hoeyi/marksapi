using Microsoft.Extensions.Logging;

namespace Ichyd.Marksapi.Cli.Extensions
{
    static class LoggerExtensions
    {
        public static void LogInfo_Services_Initializing(this ILogger? logger)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger.LogInformation("Starting service initialization...");
        }

        public static void LogInfo_Services_Initializing_Finished(this ILogger? logger)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger.LogInformation("Finished service initialization.");
        }

        public static void LogInfo_Service_Registered(this ILogger? logger, string service)
        {
            if(logger?.IsEnabled(LogLevel.Information) ?? false)
                logger.LogInformation("Registered {service}", service);
        }
    }
}

