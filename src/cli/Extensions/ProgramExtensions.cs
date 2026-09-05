using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using ApiClient.Services;
using Ichyd.Marksapi.Cli.Massive.Verbs;
using Ichyd.Marksapi.Cli.Verbs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ichyd.Marksapi.Cli.Extensions
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

        public static Command AddMassiveApiCommand(this Command command)
        {
            // Massive service subcommand
            var massiveCommand = new Command("massive", "Access Massive API endpoints")
            {
                TickerHandler.CreateCommand(),
                TickerInfoHandler.CreateCommand(),
                AggregateBarHandler.CreateCommand(),
                ShortInterestHandler.CreateCommand(),
                ShortVolumeHandler.CreateCommand(),
                InflationHandler.CreateCommand(),
                InflationForecastHandler.CreateCommand(),
                LaborHandler.CreateCommand(),
                TreasuryHandler.CreateCommand()
            };

            command.Add(massiveCommand);

            return command;
        }

        public static RootCommand InitRootCommand(IConfiguration configuration)
        {
            var rootCommand = new RootCommand()
            {
                FileReaderHandler.CreateLicenseCommand(),
                FileReaderHandler.CreateNoticeCommand()
            };
            rootCommand.Description = "A unified command line interface for querying financial data APIs.";

            for (int i = 0; i < rootCommand.Options.Count; i++)
            {
                if (rootCommand.Options[i] is VersionOption)
                    rootCommand.Options[i].Action = new CustomVersionAction();
            }

            return rootCommand;
        }        
    }
}

