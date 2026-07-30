using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ApiClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Ichyd.Marksapi.Cli
{
    [ExcludeFromCodeCoverage]
    static class ProgramExtensions
    {
        /// <summary>
        /// Gets the registered service matching <typeparamref name="T"/>, else
        /// throws and exception.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns>An instanced of <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T GetServiceOrThrow<T>(this IServiceProvider services)
        {
            return services.GetService<T>() ?? 
                throw new InvalidOperationException(
                    message: $"Service '{nameof(T)}' not found.");
        }

        /// <summary>
        /// Getss the <see cref="QueryOptions"/> section of the 'massive' parent element.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>A <see cref="QueryOptions"/> instance if found, else default having range (1, 5000).</returns>
        public static QueryOptions GetQueryOptionsOrDefault(this IConfiguration configuration)
        {   
            QueryOptions options = new();
            var section = configuration
                .GetSection("massive")?
                .GetSection(nameof(QueryOptions));
            if (section is null)
            {
                options.UpperLimit = 5000;
                options.LowerLimit = 1;
            }
            else
                section.Bind(options);

            return options;
        }

        /// <summary>
        /// Gets an <see cref="Interval{int}"/> from this <see cref="QueryOptions"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>An <see cref="Interval{int}"/>.</returns>
        public static Interval<int> QueryLimit(this QueryOptions options) => 
            new(options.LowerLimit, options.UpperLimit, open: false);        
    }

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
    /// <summary>
    /// Simple service provider.
    /// </summary>
    [ExcludeFromCodeCoverage]
    class SingletonServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _servies = [];

        /// <summary>
        /// Register a new service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public SingletonServiceProvider RegisterService<T>(T instance)
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            _servies.Add(typeof(T), instance);

            return this;
        }

        public object? GetService(Type serviceType)
        {
            if(!_servies.TryGetValue(serviceType, out object? value))
                return null;
            else
                return value;
        }
    }
}

