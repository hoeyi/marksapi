using System.CommandLine;
using ApiClient.Massive;
using ApiClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs
{
    public static class ShortVolumeHandler
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
            string format,
            int? limit,
            ILogger? logger = null,
            CancellationToken cancellationToken = default)
        {
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
                    format,
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
                    format,
                    $"./massive/{result.RequestId}",
                    cancellationToken);
            }
        }
    }
}