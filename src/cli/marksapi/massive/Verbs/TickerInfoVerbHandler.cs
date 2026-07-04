using System;
using System.CommandLine;
using System.Diagnostics;
using System.Linq;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using ApiClient.Massive.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs;

public static class TickerInfoHandler
{
        public static Command CreateCommand()
        {
            var command = new Command("ticker-info", "Retrieve comprehensive details for a single ticker");

            command
                .AddMarketArgument()
                .AddTickerArgument()
                .AddTickersOption()
                .AddDateOption()
                .AddFormatOption();

            // TODO: Register action.
            return command;
        }

        private static async Task Handle(
            IServiceProvider services,
            string market,
            string? ticker,
            string? tickers,
            DateTime? date,
            string format,
            ILogger? logger = null,
            CancellationToken cancellationToken = default)
        {
            var validator = new CommandValidator();
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
                                                format: format,
                                                path: $"./{result.RequestId}",
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
                                                    format: format,
                                                    path: $"./{res.RequestId}",
                                                    cancellationToken);
                    }
                }
            }
        }
}
