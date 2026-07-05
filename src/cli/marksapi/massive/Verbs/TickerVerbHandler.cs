// MassiveTickers.cs
using System;
using System.CommandLine;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Massive;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs
{
    public static class TickerHandler
    {
        public static Command CreateCommand()
        {
            var command = new Command("tickers", "Query for tickers matching given conditions");

            command
                .AddMarketArgument()
                .AddTickerOption()
                .AddTickerTypeOption()
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
            command.SetAction((pr, ct) =>
            {
                string? market = pr.GetValue<string>("MARKET");
                string? ticker = pr.GetValue<string>("TICKER");
                string? type = pr.GetValue<string>("--type");
                string? exchange = pr.GetValue<string>("--exchange");
                string? cusip = pr.GetValue<string>("--cusip");
                string? cik = pr.GetValue<string>("--cik");
                string? search = pr.GetValue<string>("--search");
                bool active = pr.GetValue<bool>("--active");
                bool asc = pr.GetValue<bool>("--asc");
                string? sort = pr.GetValue<string>("--sort");
                DateTime? date = pr.GetValue<DateTime?>("--date");
                string? format = pr.GetValue<string>("--format");
                int? limit = pr.GetValue<int?>("--limit");

                return Handle(
                    Program.Services,
                    market,
                    ticker,
                    type,
                    exchange,
                    cusip,
                    cik,
                    date,
                    search,
                    active,
                    asc,
                    sort,
                    limit,
                    format,
                    ct);
            });

            return command;
        }

        private static async Task Handle(
            IServiceProvider services,
            string? market,
            string? ticker,
            string? type,
            string? exchange,
            string? cusip,
            string? cik,
            DateTime? date,
            string? search,
            bool active,
            bool asc,
            string? sort,
            int? limit,
            string? format,
            CancellationToken cancellationToken = default)
        {
            var logger = services.GetServiceOrThrow<ILogger>();
            
            var validator = new CommandValidator(logger);
            validator
                .ValidateTickerOrThrow(ticker)
                .ValidateFormatOrThrow(format)
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
                            limit: limit ?? Program.QueryLimit.End,
                            cancellationToken);

            await OutputService.WriteAsync(
                    item: result.Results,
                    format: format!,
                    path: $"./{result.RequestId}",
                    cancellationToken);
        }
    }
}