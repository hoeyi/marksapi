using Microsoft.Extensions.Logging;
using System.IO;

namespace Ichyd.Marksapi.Cli.Extensions
{
    static class LoggerExtensions
    {
        public static void LogWarning_FileNotExists(this ILogger? logger, string filename)
        {
            if(logger?.IsEnabled(LogLevel.Warning) ?? false)
                logger.LogWarning("{filename} not found or is not a file.", filename);
        }

        public static void LogDebug_Services_Initializing(this ILogger? logger)
        {
            if(logger?.IsEnabled(LogLevel.Debug) ?? false)
                logger.LogDebug("Starting service initialization...");
        }

        public static void LogDebug_Services_Initializing_Finished(this ILogger? logger)
        {
            if(logger?.IsEnabled(LogLevel.Debug) ?? false)
                logger.LogDebug("Finished service initialization.");
        }

        public static void LogDebug_Service_Registered(this ILogger? logger, string service)
        {
            if(logger?.IsEnabled(LogLevel.Debug) ?? false)
                logger.LogDebug("Registered {service}", service);
        }
    }
}

