using System.CommandLine;
using ApiClient.Massive;
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
                .AddOutputOption();

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
            int limit,
            string output,
            ILogger? logger = null,
            CancellationToken cancellationToken = default)
        {
            if(
                (string.IsNullOrEmpty(ticker) & string.IsNullOrEmpty(tickers)) ||
                (string.IsNullOrWhiteSpace(ticker) & string.IsNullOrWhiteSpace(tickers)))
            {
                logger?.LogError("Either {arg1} or {arg2} must be specified.", "TICKER", "--ticker");
                throw new ArgumentException($"Parameters: {nameof(ticker)}, {nameof(tickers)}.");
            }

            if(!(fromDate.HasValue && toDate.HasValue))
            {
                logger?.LogError("Both {arg1} or {arg2} must be specified.", "--from", "--to");
                throw new ArgumentException($"Parameters: {nameof(fromDate)}, {nameof(toDate)}.");
            }

            ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 5000, paramName: nameof(limit));
            ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1, paramName: nameof(limit));

            if (limit < 1 || limit > 5000)
            {
                logger?.LogError("Both {arg1} or {arg2} must be specified.", "--from", "--to");
                throw new ArgumentException($"Parameters: {nameof(fromDate)}, {nameof(toDate)}.");
            }

            if (ratioMin.HasValue && ratioMax.HasValue && ratioMin > ratioMax)
            {
                Console.Error.WriteLine("Error: --ratio-min cannot be greater than --ratio-max");
                Environment.Exit(2);
            }

            try
            {
                var handler = services.GetService<IMassiveApi>();

                var result = await handler.GetShortVolumeResponseAsync(
                    ,
                    fromDate.Value.Date,
                    toDate.Value.Date,
                    ratioMin,
                    ratioMax,
                    limit,
                    output,
                    cancellationToken);

                result.Print();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error executing command: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}