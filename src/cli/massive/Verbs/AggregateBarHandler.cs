using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using Ichyd.Marksapi.Cli.Extensions;
using Ichyd.Marksapi.Cli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ichyd.Marksapi.Cli.Massive.Verbs
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
                .AddFileOutputOption()
                .AddUnadjustedOption();

            
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
                bool unadjusted = pr.GetValue<bool>("--unadjusted");
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
                    !unadjusted,
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
            bool adjusted,
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
                .ValidateEnumOrThrow(market!, out Market marketEnum)
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateLimitOrThrow(limit, queryLimit)
                .ValidateDateRangeOrThrow(fromDate, toDate)
                .ValidateFileOuputOrThrow(outputPath)
                .ValidateTimespanOrThrow(timespan, out BarTimespan? barTimespan);

            var handler = services.GetServiceOrThrow<IMassiveApi>();
            var tickerArgs = !string.IsNullOrEmpty(ticker) ?
                ticker.ToValueArray() : 
                tickers.ToValueArray();

            var results = await handler.GetAggregateBarResponseAsync(
                    market: marketEnum,
                    tickers: tickerArgs,
                    multiplier: multiplier,
                    timeSpan: barTimespan ?? BarTimespan.Day,
                    fromDate!.Value,
                    toDate!.Value,
                    adjusted,
                    limit ?? queryLimit.End,
                    cancellationToken);
            
            if(results.Count == 0) return;

            string path = OutputService.CombinePath(
                    outputPath ?? config["output_path"] ?? "./",
                    Guid.NewGuid().ToString());
            if(format.CompareTo("csv", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                results
                    .ForEach(x =>
                    {
                        x.Results.ForEach(y =>
                        {
                            y.RequestId = x.RequestId;
                            y.Ticker = x.Ticker;
                            y.Status = x.Status;
                            y.Adjusted = x.Adjusted;
                        });
                    });
                var flatresults = results.SelectMany(x => x.Results);

                await OutputService.WriteAsync(
                            data: flatresults,
                            format!,
                            path,
                            cancellationToken);
            }
            else
            {
                await OutputService.WriteAsync(
                            data: results,
                            format!,
                            path,
                            cancellationToken);
            }
        }
    }
}