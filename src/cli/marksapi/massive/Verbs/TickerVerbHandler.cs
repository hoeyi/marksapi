// MassiveTickers.cs
using System.CommandLine;
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
                .AddOutputOption();

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
            bool inactive,
            bool desc,
            string? sort,
            int limit,
            string output,
            CancellationToken cancellationToken = default)
        {
            if (limit < 1 || limit > 1000)
            {
                Console.Error.WriteLine($"Error: --limit must be between 1 and 1000, got {limit}");
                Environment.Exit(2);
            }

            try
            {
                var handler = services.GetKeyed<IMassiveServiceHandler>("MassiveServiceHandler");

                var result = await handler.HandleTickersAsync(
                    new TickersRequest
                    {
                        Ticker = ticker,
                        Type = type,
                        Market = market,
                        Exchange = exchange,
                        Cusip = cusip,
                        Cik = cik,
                        Date = date,
                        Search = search,
                        IncludeInactive = inactive,
                        SortDescending = desc,
                        SortField = sort,
                        Limit = limit
                    },
                    output,
                    cancellationToken);

                result.Print();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error executing command: {ex.Message}");
                Environment.Exit(1);
            }
        }

        private sealed class TickersRequest
        {
            public string? Ticker { get; init; }
            public string? Type { get; init; }
            public string? Market { get; init; }
            public string? Exchange { get; init; }
            public string? Cusip { get; init; }
            public string? Cik { get; init; }
            public DateTime? Date { get; init; }
            public string? Search { get; init; }
            public bool IncludeInactive { get; init; }
            public bool SortDescending { get; init; }
            public string? SortField { get; init; }
            public int Limit { get; init; } = 100;
        }
    }
}