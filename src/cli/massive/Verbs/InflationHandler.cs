using ApiClient.Massive;
using ApiClient.Massive.Parameters;
using Ichyd.Marksapi.Cli.Extensions;
using Ichyd.Marksapi.Cli.Massive.Verbs;
using Ichyd.Marksapi.Cli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Ichyd.Marksapi.Cli.Massive.Verbs
{
    [ExcludeFromCodeCoverage]
    class InflationHandler
    {
        public static Command CreateCommand()
        {
            var command = new Command(
                "inflation",
                "Retreive US historical inflation");

            command
                .AddDateArrayOption()
                .AddComparisonArrayOption()
                .AddFormatOption()
                .AddLimitOption()
                .AddFileOutputOption();
        
            command.SetAction((pr, ct) =>
            {
                DateTime[]? dates = pr.GetValue<DateTime[]>("--date") ?? [];
                var ops = pr.GetValue<NumericComparisonOperator[]>("--operator");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");
                string? outputPath = pr.GetValue<string>("--to-file");

                var dateArgs = CommandBuilder.ConvertNumericArguments(dates, ops);

                return Handle(
                    Program.Services,
                    dateArgs,
                    format,
                    limit,
                    outputPath,
                    ct);
            });

            return command;
        }

        private static async Task Handle(
                IServiceProvider services,
                Dictionary<NumericComparisonOperator, DateTime>? dateFilters,
                string? format,
                int? limit,
                string? outputPath,
                CancellationToken cancellationToken = default)
        {
            var logger = services.GetServiceOrThrow<ILogger>();
            var config = services.GetServiceOrThrow<IConfiguration>();
            var queryLimit = config
                            .GetQueryOptionsOrDefault()
                            .QueryLimit();

            var validator = new CommandValidator(logger);
            validator
                .ValidateFormatOrThrow(format)
                .ValidateLimitOrThrow(limit, queryLimit)
                .ValidateFileOuputOrThrow(outputPath);
            
            var handler = services.GetServiceOrThrow<IMassiveApi>();

            var results = await handler.GetInflationResponseAsync(
                                    dateFilters,
                                    limit,
                                    cancellationToken);

            string path = OutputService.CombinePath(
                    outputPath ?? config["output_path"] ?? "./",
                    Guid.NewGuid().ToString());

            if(format.CompareTo("csv", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                results.Results.ForEach(x =>
                {
                    x.RequestId = results.RequestId;
                    x.Status = results.Status;
                });

                await OutputService.WriteAsync(
                    data: results.Results,
                    format!,
                    path,
                    cancellationToken);
            }
            else
            {
                await OutputService.WriteAsync(
                                data: results,
                                format!,
                                path,
                                cancellationToken);                                  
            }
        }
    }
}

