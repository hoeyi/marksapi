using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;

namespace Ichyd.Marksapi.Cli.Extensions
{
    static class LoggerExtensions
    {
        public static void LogFatal_UnhandledException(
            this ILogger? logger,
            Exception e,
            bool terminating)
        {
            if(logger?.IsEnabled(LogLevel.Critical) ?? false)
                logger.LogCritical(
                    eventId: 10,
                    "Encountered unhandled exception.\n{e.Message}\n{e.StackTrace}",
                    e.Message,
                    e.StackTrace);
        }

        public static void LogFatal_ErrorDuringStartup(this ILogger? logger, Exception exception)
        {
            if(logger?.IsEnabled(LogLevel.Critical) ?? false)
                logger.LogCritical(
                    eventId: 11,
                    "Error during startup: {message}\n{stack_trace}",
                    exception.Message,
                    exception.StackTrace);
        }

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

