// MassiveTickers.cs
using System;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using Marksapi.Cli;

namespace Marksapi.Cli.Massive.Verbs
{
    public static class TickerHandler
    {
        public static Command CreateCommand()
        {
            var command = new Command("tickers", "Query for tickers matching given conditions");

            command
                .AddTickerOption()
                .AddTickerTypeOption()
                .AddMarketOption()
                .AddExchangeOption()
                .AddCusipOption()
                .AddCikOption()
                .AddDateOption()
                .AddSearchOption()
                .AddInactiveOption()
                .AddSortDescendingOption()
                .AddSortFieldOption()
                .AddLimitOption()
                .AddFormatOption();

            // TODO: Register action.
            return command;
        }

        private static async Task Handle(
            IServiceProvider services,
            string? ticker,
            string? type,
            string? market,
            string? exchange,
            string? cusip,
            string? cik,
            DateTime? date,
            string? search,
            bool active,
            bool asc,
            string? sort,
            int limit,
            string format,
            CancellationToken cancellationToken = default)
        {
            var validator = new CommandValidator();
            validator
                .ValidateTickerOrThrow(ticker)
                .ValidateLimitOrThrow(limit, Program.QueryLimit);

            var handler = services.GetServiceOrThrow<IMassiveApi>();

            var result = await handler.GetAllTickersAsync(
                            ticker: ticker,
                            type: type,
                            market: market,
                            exchange: exchange,
                            cusip: cusip,
                            cik: cik,
                            date: date,
                            search: search,
                            active: active,
                            asc: asc,
                            sort: sort,
                            limit: limit,
                            cancellationToken);

            await OutputService.WriteAsync(
                    item: result.Results,
                    format: format,
                    path: $"./{result.RequestId}",
                    cancellationToken);
        }
    }
}