using System;
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
                .AddRatioMinOption()
                .AddRatioMaxOption()
                .AddLimitOption()
                .AddFormatOption()
                .AddFileOutputOption();

            // TODO: Register action.
            command.SetAction((pr, ct) =>
            {
                string? tickers = pr.GetValue<string>("--tickers");
                DateTime? settlementDate = pr.GetValue<DateTime?>("--settlement");
                float? daysToCover = pr.GetValue<float?>("--days-to-cover");
                var op = pr.GetValue<string>("--operator");
                float? avgVolume = pr.GetValue<float?>("--avg-volume");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");
                string? outputPath = pr.GetValue<string>("--to-file");

                return Handle(
                    Program.Services,
                    tickers,
                    settlementDate,
                    null,
                    null,
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
            (float, string)? daysToCover,
            (float, string)? averageDailyVolume,
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
            
            var result = await handler.GetShortInterestResponseAsync(
                tickers.ToValueArray(),
                settlementDate,
                // TODO: Fix this
                null,
                null,
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