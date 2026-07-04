// MassAggregationBar.cs
using System.CommandLine;

namespace Marksapi.Cli.Massive.Verbs
{
    public static class AggregateBarHandler
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
                .AddOutputOption();

            // TODO: Register action.
            return command;
        }

        private static async Task Handle(
            IServiceProvider services, string market, string? ticker, int multiplier, string timespan, DateTime from,
            DateTime to, int limit, string? tickers, string output, CancellationToken cancellationToken = default)
        {
            // Validate required arguments
            if (string.IsNullOrWhiteSpace(ticker) && string.IsNullOrWhiteSpace(tickers))
            {
                Console.Error.WriteLine("Error: Either TICKER or --tickers must be specified");
                return;
            }

            // Process validation constraints
            if (limit < 1 || limit > 1000)
            {
                Console.Error.WriteLine($"Error: --limit must be between 1 and 1000, got {limit}");
                Environment.Exit(2);
            }

            if (!IsValidTimespan(timespan))
            {
                Console.Error.WriteLine($"Error: Invalid timespan '{timespan}'. Valid options: day, week, month, hour, minute");
                Environment.Exit(2);
            }

            try
            {
                var handler = services.GetKeyed<IMassiveServiceHandler>("MassiveServiceHandler");
                
                var result = await handler.HandleAggregateBarAsync(
                    market,
                    GetTickers(ticker, tickers),
                    multiplier,
                    timespan,
                    from.Date,
                    to.Date,
                    limit,
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

        
    }
}