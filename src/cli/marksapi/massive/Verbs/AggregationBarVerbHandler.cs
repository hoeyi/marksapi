using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using Marksapi.Cli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs
{
    [ExcludeFromCodeCoverage]
    static class AggregateBarHandler
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
                .AddFormatOption()
                .AddFileOutputOption();

            
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
                string? outputPath = pr.GetValue<string>("--to-file");

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
                    outputPath,
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
            string? outputPath,
            CancellationToken cancellationToken = default)
        {
            var logger = services.GetServiceOrThrow<ILogger>();
            var config = services.GetServiceOrThrow<IConfiguration>();
            
            var validator = new CommandValidator(logger);
            validator
                .ValidateMarketOrThrow(market, out Market mktEnum)
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateLimitOrThrow(limit, Program.QueryLimit)
                .ValidateDateRangeOrThrow(fromDate, toDate)
                .ValidateFileOuputOrThrow(outputPath)
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