using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Marksapi.Cli
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

            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            var runtimeVersion = Environment.Version;

            Console.WriteLine($"App Version: {appVersion}");
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

