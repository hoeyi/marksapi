using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using Ichyd.Marksapi.Cli.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ichyd.Marksapi.Cli.Massive.Verbs
{
    [ExcludeFromCodeCoverage]
    static class TickerInfoHandler
    {
        public static Command CreateCommand()
        {
            var command = new Command("ticker-info", "Retrieve comprehensive details for a single ticker");

            command
                .AddMarketArgument()
                .AddTickerArgument()
                .AddTickersOption()
                .AddDateOption()
                .AddFormatOption()
                .AddFileOutputOption();

            // TODO: Register action.
            command.SetAction((pr, ct) =>
            {
                string? market = pr.GetValue<string>("MARKET");
                string? ticker = pr.GetValue<string>("TICKER");
                string? tickers = pr.GetValue<string>("--tickers");
                DateTime? date = pr.GetValue<DateTime?>("--date");
                DateTime? toDate = pr.GetValue<DateTime?>("--to");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");
                string? outputPath = pr.GetValue<string>("--to-file");

                return Handle(
                    Program.Services,
                    market,
                    ticker,
                    tickers,
                    date,
                    format,
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
            DateTime? date,
            string? format,
            string? outputPath,
            CancellationToken cancellationToken = default)
        {
            var logger = services.GetServiceOrThrow<ILogger>();
            var config = services.GetServiceOrThrow<IConfiguration>();

            var validator = new CommandValidator(logger);
            validator
                .ValidateMarketOrThrow(market, out Market mktEnum)
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateFormatOrThrow(format)
                .ValidateFileOuputOrThrow(outputPath);
            
            var tickerArgs = !string.IsNullOrEmpty(ticker) ?
                ticker.ToValueArray() : 
                tickers.ToValueArray();
            var handler = services.GetServiceOrThrow<IMassiveApi>();

            var results = await handler.GetTickerOverviewResponseAsync(
                    market: mktEnum,
                    tickers: tickerArgs,
                    date: date,
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
                        x.Results?.RequestId = x.RequestId;
                    });

                var flatresults = results.Select(x => x.Results);

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

