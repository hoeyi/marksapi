using System;
using System.CommandLine;
using System.Diagnostics;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using ApiClient.Massive;
using ApiClient.Massive.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

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
                .AddOutputOption();

            // TODO: Register action.
            return command;
        }

        private static async Task Handle(
            IServiceProvider services,
            string market,
            string? ticker,
            string? tickers,
            DateTime? date,
            string output,
            ILogger? logger = null,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(market);
            if(
                (string.IsNullOrEmpty(ticker) & string.IsNullOrEmpty(tickers)) ||
                (string.IsNullOrWhiteSpace(ticker) & string.IsNullOrWhiteSpace(tickers)))
            {
                logger?.LogError("Either {arg1} or {arg2} must be specified.", "TICKER", "--ticker");
                throw new ArgumentException($"Parameters: {nameof(ticker)}, {nameof(tickers)}.");
            }

            Market? mkt = IMassiveApi.ParseEnumOrThrow<Market>(market);

            try
            {
                var handler = services.GetService<IMassiveApi>() ?? 
                    throw new InvalidOperationException($"Service '{nameof(IMassiveApi)}' not found.");

                if (!string.IsNullOrEmpty(ticker))
                {
                    var result = await handler.GetTickerOverviewResponseAsync(
                        mkt!.Value,
                        ticker,
                        date,
                        cancellationToken);

                    if(result.Results is TickerOverview tovw)
                    {
                        var writeResult = await OutputService.WriteAsync(
                                                    item: tovw,
                                                    format: output,
                                                    path: $"./{result.RequestId}",
                                                    cancellationToken);
                    }
                }
                else if (!string.IsNullOrEmpty(tickers))
                {
                    var result = await handler.GetTickerOverviewResponseAsync(
                        mkt!.Value,
                        tickers.Split(","),
                        date,
                        cancellationToken);

                    var resultNotNull = result.Where(x => x.Results is not null).ToArray();

                    if(resultNotNull.Length > 0)
                    {
                        foreach(var res in resultNotNull)
                        {
                            var writeResult = await OutputService.WriteAsync(
                                                        item: res.Results!,
                                                        format: output,
                                                        path: $"./{res.RequestId}",
                                                        cancellationToken);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error executing command: {ex.Message}");
                Environment.Exit(1);
            }
        }
}


