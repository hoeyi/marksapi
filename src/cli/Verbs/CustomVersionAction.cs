using Spectre.Console;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Ichyd.Marksapi.Cli.Verbs
{
    /// <summary>
    /// Custom version action that prints application title in ANSI format.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class CustomVersionAction : SynchronousCommandLineAction
    {
        /// <inheritdoc/>
        public override int Invoke(ParseResult parseResult)
        {
            AnsiConsole.Write(new FigletText(nameof(Marksapi).ToLower()));
            AnsiConsole.WriteLine(
                $"\n{parseResult.RootCommandResult.Command.Description!}");

            var appInfoVersion = Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

            var appBuildDateStr = Assembly
                .GetExecutingAssembly()
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(x => x.Key == "BuildDate")
                ?.Value;
            bool hasBuildDate = DateTime.TryParseExact(
                appBuildDateStr,
                "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime buildDate);
            
            var runtimeVersion = Environment.Version;

            Console.WriteLine($"App Version: {appInfoVersion}");
            if(hasBuildDate)
                Console.WriteLine($"Build date: {buildDate:yyyyMMdd-HHmmss}");
            Console.WriteLine($".NET Runtime Version: {runtimeVersion}");
            return 0;
        }
    }
}
