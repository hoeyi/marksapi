using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using ApiClient.Massive.Response;
using ApiClient.Massive.Response.Stocks;
using Ichyd.Marksapi.Cli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Spectre.Console;

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

            var results = await handler.GetAggregateBarResponseAsync(
                    market: mktEnum,
                    tickers: tickerArgs,
                    multiplier: multiplier,
                    timeSpan: barTimespan ?? BarTimespan.Day,
                    fromDate!.Value,
                    toDate!.Value,
                    limit ?? Program.QueryLimit.End,
                    cancellationToken);
            
            var writeTasks = new List<Task>();
            string path = default!;
            foreach(var result in results)
            {
                if(string.IsNullOrEmpty(result.RequestId))
                    continue;
                else
                    path = OutputService.CombinePath(
                        outputPath ?? config["output_path"] ?? "./",
                        result.RequestId);
                    

                    // For CSV, we want flattened results for the response.
                    
                    if(format.CompareTo("csv", StringComparison.InvariantCultureIgnoreCase) == 0)
                    {
                        result.Results.ForEach(x =>
                        {
                            x.Ticker = result.Ticker;
                            x.Adjusted = result.Adjusted; 
                            x.Status = result.Status;
                        });
                        writeTasks.Add(
                            Task.Run(
                                () => OutputService.WriteAsync(
                                    data: [.. result.Results],
                                    format!,
                                    path,
                                    cancellationToken)));
                    }
                    // For preserving JSON response, write single response result otherwise.
                    else
                    {
                        writeTasks.Add(
                            Task.Run(
                                () => OutputService.WriteAsync(
                                    item: result,
                                    format!,
                                    path,
                                    cancellationToken)));
                        
                    }
            };

            await Task.WhenAll(writeTasks);
        }
    }
}