// MassAggregationBar.cs
using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs
{
    public static class AggregateBarHandler
    {
        public static Command CreateCommand()
        {
            var command = new Command("aggregate-bar", "Retrieve aggregated historical OHLC and volume data");
            
            command
                .AddMarketArgument()
                .AddTickerArgument()
                .AddMultiplierOption()
                .AddTimespanOption()
                .AddFromDateOption()
                .AddToDateOption()
                .AddLimitOption()
                .AddTickersOption()
                .AddFormatOption();

            
            // TODO: Register action.
            command.SetAction((pr, ct) =>
            {
                string? market = pr.GetValue<string>("MARKET");
                string? ticker = pr.GetValue<string>("TICKER");
                string? tickers = pr.GetValue<string>("--tickers");
                int multiplier = pr.GetValue<int>("--multiplier");
                string? timespan = pr.GetValue<string>("--timespan");
                DateTime? fromDate = pr.GetValue<DateTime?>("--from");
                DateTime? toDate = pr.GetValue<DateTime?>("--to");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");

                return Handle(
                    Program.Services,
                    market,
                    ticker,
                    tickers,
                    multiplier,
                    timespan,
                    fromDate,
                    toDate,
                    format,
                    limit,
                    ct);
            });

            return command;
        }

        private static async Task Handle(
            IServiceProvider services,
            string? market,
            string? ticker,
            string? tickers,
            int multiplier,
            string? timespan,
            DateTime? fromDate,
            DateTime? toDate,
            string? format,
            int? limit,
            CancellationToken cancellationToken = default)
        {
            var logger = services.GetServiceOrThrow<ILogger>();

            var validator = new CommandValidator(logger);
            validator
                .ValidateMarketOrThrow(market, out Market mktEnum)
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateLimitOrThrow(limit, Program.QueryLimit)
                .ValidateDateRangeOrThrow(fromDate, toDate)
                .ValidateTimespanOrThrow(timespan, out BarTimespan? barTimespan);

            var handler = services.GetServiceOrThrow<IMassiveApi>();
            var tickerArgs = !string.IsNullOrEmpty(ticker) ?
                ticker.ToValueArray() : 
                tickers.ToValueArray();

            var result = await handler.GetAggregateBarResponseAsync(
                    market: mktEnum,
                    tickers: tickerArgs,
                    multiplier: multiplier,
                    timeSpan: barTimespan ?? BarTimespan.Day,
                    fromDate!.Value,
                    toDate!.Value,
                    limit ?? Program.QueryLimit.End,
                    cancellationToken);

                await OutputService.WriteAsync(
                    result.Results,
                    format!,
                    $"./massive/{result.RequestId}",
                    cancellationToken);
        }
    }
}