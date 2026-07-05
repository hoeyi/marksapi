using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using ApiClient.Services;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs
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
                .AddFormatOption();

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
            CancellationToken cancellationToken = default)
        {
            var logger = services.GetServiceOrThrow<ILogger>();
            
            var validator = new CommandValidator(logger);
            validator 
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateDateRangeOrThrow(fromDate, toDate)
                .ValidateRatioRangeOrThrow(ratioMin, ratioMax)
                .ValidateFormatOrThrow(format)
                .ValidateLimitOrThrow(limit, Program.QueryLimit);

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

                await OutputService.WriteAsync(
                    result.Results,
                    format!,
                    $"./massive/{result.RequestId}",
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

                await OutputService.WriteAsync(
                    result.Results,
                    format!,
                    $"./massive/{result.RequestId}",
                    cancellationToken);
            }
        }
    }
}