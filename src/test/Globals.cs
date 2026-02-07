global using System;
global using Xunit;
// using Serilog;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ApiClient.Marketstack.xUnitTests
{
    /// <summary>
    /// Defines names of attributes applied to objects via TraitAttribute.
    /// </summary>
    record TestAttributeNames
    {
        public string Category { get; } = default!;
    }

    static class TestRun
    {
        public static Microsoft.Extensions.Logging.ILogger  Logger {get;} = CreateLogger();
        
        private static Microsoft.Extensions.Logging.ILogger CreateLogger()
        {

            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File(".log", rollingInterval: RollingInterval.Day)
                    .CreateLogger();

            var loggerFactory = new LoggerFactory().AddSerilog(Log.Logger);

            return loggerFactory.CreateLogger(categoryName: $"Test.Logger.{nameof(MarketstackApi)}");
        }
    }
}