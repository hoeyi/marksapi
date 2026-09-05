using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using ApiClient.Massive.Parameters;
using ApiClient.Services;
using Ichyd.Marksapi.Cli.Extensions;
using Ichyd.Marksapi.Cli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ichyd.Marksapi.Cli.Massive.Verbs
{
    [ExcludeFromCodeCoverage]
    static class ShortVolumeHandler
    {
        public static Command CreateCommand()
        {
            var command = new Command(
                "short-volume",
                "Retrieve daily aggregated short sale volume data reported to FINRA");

            command
                .AddTickersOption()
                .AddDateArrayOption()
                .AddShortDailyVolumeOptions()
                .AddComparisonArrayOption()
                .AddLimitOption()
                .AddFormatOption()
                .AddFileOutputOption();

            // TODO: Register action.
            command.SetAction((pr, ct) =>
            {
                string? tickers = pr.GetValue<string>("--tickers");
                DateTime[]? dateFilters = pr.GetValue<DateTime[]>("--date");
                float[]? ratioFilters = pr.GetValue<float[]>("--short-volume-ratio");
                var ops = pr.GetValue<NumericComparisonOperator[]>("--operator");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");
                string? outputPath = pr.GetValue<string>("--to-file");

                var dateArgs = CommandBuilder
                                .ConvertNumericArguments(dateFilters, ops);
                var ratioArgs = CommandBuilder.ConvertNumericArguments(
                                    ratioFilters,
                                    ops,
                                    offset: dateArgs?.Count ?? 0);

                return Handle(
                    Program.Services,
                    tickers,
                    dateArgs,
                    ratioArgs,
                    format,
                    limit,
                    outputPath,
                    ct
                );
            });

            return command;
        }

        private static async Task Handle(
            IServiceProvider services,
            string? tickers,
            Dictionary<NumericComparisonOperator, DateTime>? dateFilters,
            Dictionary<NumericComparisonOperator, float>? ratioFilters,
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
                .ValidateFileOuputOrThrow(outputPath)
                .ValidateLimitOrThrow(limit, queryLimit);

            var handler = services.GetServiceOrThrow<IMassiveApi>();
            
            var result = await handler.GetShortVolumeResponseAsync(
                tickers.ToValueArray(),
                dateFilter: dateFilters,
                shortVolumeRatio: ratioFilters,
                limit,
                cancellationToken);

            var path = OutputService.CombinePath(
                    outputPath ?? config["output_path"] ?? "./",
                    result.RequestId);
            await OutputService.WriteAsync(
                result.Results,
                format!,
                path,
                cancellationToken);
        }
    }
}