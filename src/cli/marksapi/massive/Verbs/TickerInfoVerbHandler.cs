using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using ApiClient.Massive.Response;
using Marksapi.Cli.Services;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs
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
        
            var validator = new CommandValidator(logger);
            validator
                .ValidateMarketOrThrow(market, out Market mktEnum)
                .ValidateTickerOrTickersOrThrow(ticker, tickers)
                .ValidateFormatOrThrow(format);

            var handler = services.GetServiceOrThrow<IMassiveApi>();

            if (!string.IsNullOrEmpty(ticker))
            {
                var result = await handler.GetTickerOverviewResponseAsync(
                    mktEnum,
                    ticker,
                    date,
                    cancellationToken);

                if(result.Results is TickerOverview tovw)
                {
                    var writeResult = await OutputService.WriteAsync(
                                                item: tovw,
                                                format: format!,
                                                path: $"./massive/{result.RequestId}",
                                                cancellationToken);
                }
            }
            else if (!string.IsNullOrEmpty(tickers))
            {
                var result = await handler.GetTickerOverviewResponseAsync(
                    market: mktEnum,
                    tickers: tickers.ToValueArray(),
                    date: date,
                    cancellationToken);

                var resultNotNull = result.Where(x => x.Results is not null).ToArray();

                if(resultNotNull.Length > 0)
                {
                    foreach(var res in resultNotNull)
                    {
                        var writeResult = await OutputService.WriteAsync(
                                                    item: res.Results!,
                                                    format: format!,
                                                    path: $"./massive/{res.RequestId}",
                                                    cancellationToken);
                    }
                }
            }
        }
    }
}

