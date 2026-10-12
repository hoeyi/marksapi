using Spectre.Console;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ichyd.Marksapi.Cli.Verbs
{
    /// <summary>
    /// Custom version action that prints application title in ANSI format.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class CustomVersionAction : SynchronousCommandLineAction
    {
        private const string BUILD_DATE_KEY = "BUILD_DATE";
        
        private const string GIT_COMMIT_KEY = "GIT_COMMIT";

        /// <inheritdoc/>
        public override int Invoke(ParseResult parseResult)
        {
            AnsiConsole.Write(new FigletText(nameof(Marksapi).ToLower()));
            AnsiConsole.WriteLine(
                $"\n{parseResult.RootCommandResult.Command.Description!}");


            var customAttr = Assembly
                .GetExecutingAssembly()
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .Where(x => x.Key == "BUILD_DATE" || x.Key == "GIT_COMMIT")
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            bool hasBuildDate = DateTime.TryParseExact(
                customAttr[BUILD_DATE_KEY],
                "yyyyMMddHHmmss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime buildDate);
            
            var appInfoVersion = Assembly
                .GetExecutingAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                ?.InformationalVersion;

            var runtimeVersion = Environment.Version;

            string? commitModifier = customAttr[GIT_COMMIT_KEY];
            var appInfoCommit = string.IsNullOrEmpty(commitModifier) | 
                                    // if already tagged, don't append the commit again
                                    (appInfoVersion?.Contains('+') ?? false) ? 
                                    appInfoVersion : $"{appInfoVersion}+{commitModifier}";

            var sb = new StringBuilder();
            sb.AppendLine($"App Version: {appInfoCommit}");
            
            if(hasBuildDate)
                sb.AppendLine($"Build date: {buildDate:yyyyMMdd-HHmmss}");
            sb.AppendLine($".NET Runtime Version: {runtimeVersion}");

            Console.WriteLine(sb.ToString());
            return 0;
        }
    }
}
