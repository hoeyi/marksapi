// MassAggregationBar.cs
using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;

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
            return command;
        }

        private static async Task Handle(
            IServiceProvider services,
            string market,
            string? ticker,
            string? tickers,
            int multiplier,
            string timespan,
            DateTime? fromDate,
            DateTime? toDate,
            string format,
            int? limit,
            CancellationToken cancellationToken = default)
        {
            var validator = new CommandValidator();
            validator
                .ValidateMarketOrThrow(market, out Market mktEnum)
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateLimitOrThrow(limit, Program.QueryLimit)
                .ValidateDateRangeOrThrow(fromDate, toDate)
                .ValidateTimespanOrThrow(timespan, out BarTimespan barTimespan);

            var handler = services.GetServiceOrThrow<IMassiveApi>();
            var tickerArgs = !string.IsNullOrEmpty(ticker) ?
                ticker.ToValueArray() : 
                tickers.ToValueArray();

            var result = await handler.GetAggregateBarResponseAsync(
                    market: mktEnum,
                    tickers: tickerArgs,
                    multiplier: multiplier,
                    timeSpan: barTimespan,
                    fromDate!.Value,
                    toDate!.Value,
                    limit ?? Program.QueryLimit.End,
                    cancellationToken);

                await OutputService.WriteAsync(
                    result.Results,
                    format,
                    $"./massive/{result.RequestId}",
                    cancellationToken);
        }
    }
}