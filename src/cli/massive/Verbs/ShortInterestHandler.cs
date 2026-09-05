using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
    static class ShortInterestHandler
    {
        public static Command CreateCommand()
        {
            var command = new Command(
                "short-interest",
                "Retrieve daily short interest data reported to FINRA");

            command
                .AddTickersOption()
                .AddSettlementDateOption()
                .AddDaysToCoverOptions()
                .AddAverageDailyVolumeOptions()
                .AddComparisonOptions()
                .AddLimitOption()
                .AddFormatOption()
                .AddFileOutputOption();

            // TODO: Register action.
            command.SetAction((pr, ct) =>
            {
                string? tickers = pr.GetValue<string>("--tickers");
                DateTime? settlementDate = pr.GetValue<DateTime?>("--settlement");
                float[]? daysToCover = pr.GetValue<float[]>("--days-to-cover");
                var ops = pr.GetValue<NumericComparisonOperator[]>("--operator");
                float[]? avgVolumes = pr.GetValue<float[]>("--avg-volume");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");
                string? outputPath = pr.GetValue<string>("--to-file");

                var dtcArgs = CommandBuilder.ConvertNumericArguments(
                                daysToCover, ops);
                var volArgs = CommandBuilder.ConvertNumericArguments(
                                avgVolumes,
                                ops,
                                offset: dtcArgs?.Count ?? 0);
                return Handle(
                    Program.Services,
                    tickers,
                    settlementDate,
                    dtcArgs,
                    volArgs,
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
            DateTime? settlementDate,
            Dictionary<NumericComparisonOperator, float>? dtcFilter,
            Dictionary<NumericComparisonOperator, float>? advFilter,
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

            // var dtcArgs = daysToCover?.ToDictionary(kv =>
            //     {
            //         validator.ValidateEnumOrThrow(
            //             kv.Key, out NumericComparisonOperator numOp);
            //         return numOp;
            //     },
            //     kv => kv.Value);
            // var volArgs = averageDailyVolume?.ToDictionary(kv =>
            //     {
            //         validator.ValidateEnumOrThrow(
            //             kv.Key, out NumericComparisonOperator numOp);
            //         return numOp;
            //     },
            //     kv => kv.Value);

            var handler = services.GetServiceOrThrow<IMassiveApi>();
            
            var result = await handler.GetShortInterestResponseAsync(
                tickers.ToValueArray(),
                settlementDate,
                dtcFilter,
                advFilter,
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