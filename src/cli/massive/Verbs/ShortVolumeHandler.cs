using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
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
            var command = new Command("short-volume", "Retrieve daily aggregated short sale volume data reported to FINRA");

            command
                .AddTickerArgument()
                .AddTickersOption()
                .AddFromDateOption()
                .AddToDateOption()
                .AddRatioMinOption()
                .AddRatioMaxOption()
                .AddLimitOption()
                .AddFormatOption()
                .AddFileOutputOption();

            // TODO: Register action.
            command.SetAction((pr, ct) =>
            {
                string? ticker = pr.GetValue<string>("TICKER");
                string? tickers = pr.GetValue<string>("--tickers");
                DateTime? fromDate = pr.GetValue<DateTime?>("--from");
                DateTime? toDate = pr.GetValue<DateTime?>("--to");
                float? ratioMin = pr.GetValue<float?>("--ratio-min");
                float? ratioMax = pr.GetValue<float?>("--ratio-max");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");
                string? outputPath = pr.GetValue<string>("--to-file");

                return Handle(
                    Program.Services,
                    ticker,
                    tickers,
                    fromDate,
                    toDate,
                    ratioMin,
                    ratioMax,
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
            string? ticker,
            string? tickers,
            DateTime? fromDate,
            DateTime? toDate,
            float? ratioMin,
            float? ratioMax,
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
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateDateRangeOrThrow(fromDate, toDate)
                .ValidateRatioRangeOrThrow(ratioMin, ratioMax)
                .ValidateFormatOrThrow(format)
                .ValidateFileOuputOrThrow(outputPath)
                .ValidateLimitOrThrow(limit, queryLimit);

            var handler = services.GetServiceOrThrow<IMassiveApi>();
            
            Interval<float>? interval = ratioMin.HasValue && ratioMax.HasValue ? 
                new Interval<float>(ratioMin.Value, ratioMax.Value, open: true) : null;

            if (!string.IsNullOrEmpty(ticker))
            {
                var result = await handler.GetShortVolumeResponseAsync(
                    ticker,
                    fromDate!.Value.Date,
                    toDate!.Value.Date,
                    interval,
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
            else if (!string.IsNullOrEmpty(tickers))
            {
                var result = await handler.GetShortVolumeResponseAsync(
                    tickers.ToValueArray(),
                    fromDate!.Value.Date,
                    toDate!.Value.Date,
                    interval,
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
}