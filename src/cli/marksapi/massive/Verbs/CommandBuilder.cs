using System;
using System.CommandLine;

namespace Marksapi.Cli.Massive.Verbs
{
    static class CommandBuilder
    {
        public static Command AddMarketArgument(this Command command)
        {
            var marketArgument = new Argument<string>(name: "MARKET")
            {
                Arity = ArgumentArity.ExactlyOne
            };
            command.Add(marketArgument);
            
            return command;
        }

        public static Command AddTickerArgument(this Command command)
        {
            var tickerArgument = new Argument<string>(name: "TICKER")
            {
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(tickerArgument);

            return command;
        }

        public static Command AddMarketOption(this Command command)
        {
            var exchangeOption = new Option<string>("--exchange")
            {
                Description = "Primary exchange MIC (ISO 10383)"
            };
            command.Add(exchangeOption);

            return command;
        }

        public static Command AddCikOption(this Command command)
        {
            var cikOption = new Option<string>("--cik")
            {
                Description = "Central Index Key filter"
            };
            command.Add(cikOption);

            return command;
        }

        public static Command AddCusipOption(this Command command)
        {
            var cusipOption = new Option<string>("--cusip")
            {
                Description = "CUSIP code filter"
            };
            command.Add(cusipOption);

            return command;
        }

        public static Command AddExchangeOption(this Command command)
        {
            var exchangeOption = new Option<string>(name: "--exchange")
            {
                Description = "Primary exchange MIC (ISO 10383)"
            };
            command.Add(exchangeOption);

            return command;
        }

        public static Command AddInactiveOption(this Command command)
        {
            var inactiveOption = new Option<bool>("--inactive")
            {
                Description = "Include inactive tickers"
            };
            command.Add(inactiveOption);

            return command;
        }

        public static Command AddLimitOption(this Command command)
        {
            var limitOption = new Option<int>(name: "--limit")
            {
                Description = "Maximum records to return",
                DefaultValueFactory = new((args) => 100)
            };
            command.Add(limitOption);

            return command;
        }

        public static Command AddMultiplierOption(this Command command)
        {
            var multiplierOption = new Option<int>(name: "--multiplier")
            {
                Description = "Timespan multiplier (e.g., 1 for 1 day, 5 for 5 days)",
                Arity = ArgumentArity.ExactlyOne
            };
            command.Add(multiplierOption);

            return command;
        }

        public static Command AddOutputOption(this Command command)
        {
            var outputOption = new Option<string>(name: "--output")
            {
                Description = "Output format (json, csv)",
                DefaultValueFactory = new((args) => "json")
            };
            command.Add(outputOption);

            return command;
        }
        
        public static Command AddRatioMinOption(this Command command)
        {
            var ratioMinOption = new Option<float>(name: "--ratio-min")
            {
                Description = "Minimum short volume ratio filter"
            };
            command.Add(ratioMinOption);

            return command;
        }

        public static Command AddRatioMaxOption(this Command command)
        {
            var ratioMaxOption = new Option<float>(name: "--ratio-max")
            {
                Description = "Maximum short volume ratio filter"
            };
            command.Add(ratioMaxOption);

            return command;
        }

        public static Command AddSearchOption(this Command command)
        {
            var searchOption = new Option<string>("--search")
            {
                Description = "Search within ticker/company name"
            };
            command.Add(searchOption);

            return command;
        }

        public static Command AddSortDescendingOption(this Command command)
        {
            var sortOption = new Option<string>(name: "--desc")
                {
                    Description = "Sort descending"
                };
                command.Add(sortOption);

            return command;
        }

        public static Command AddSortFieldOption(this Command command)
        {
            var sortOption = new Option<string>(name: "--sort")
                {
                    Description = "Field to sort results by"
                };
                command.Add(sortOption);

            return command;
        }

        public static Command AddTickerOption(this Command command)
        {
            var tickerArgument = new Argument<string>(name: "TICKER")
            {
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(tickerArgument);

            return command;
        }

        public static Command AddTickersOption(this Command command)
        {
            var tickerOption = new Option<string>(name: "--tickers")
            {
                Description = "Multiple comma-separated ticker symbols"
            };
            command.Add(tickerOption);

            return command;
        }

        public static Command AddTickerTypeOption(this Command command)
        {
            var typeOption = new Option<string>(name: "--type")
            {
                Description = "Filter by ticker type"
            };
            command.Add(typeOption);

            return command;
        }

        public static Command AddTimespanOption(this Command command)
        {
            var timespanOption = new Option<string>(name: "--timespan")
            {
                Description = "Time window size (day, week, month, hour, minute)",
                Arity = ArgumentArity.ExactlyOne
            };
            command.Add(timespanOption);

            return command;
        }

        public static Command AddDateOption(this Command command)
        {
            var dateOption = new Option<DateTime>(name: "--date")
            {
                Description = "Snapshot date (YYYY-MM-DD)"
            };
            command.Add(dateOption);

            return command;
        }

        public static Command AddFromDateOption(this Command command)
        {
            var fromDateOption = new Option<DateTime>(name: "--from" )
            {
                Description = "Start date of time window (ISO format: YYYY-MM-DD)",
                Arity = ArgumentArity.ExactlyOne
            };
            command.Add(fromDateOption);

            return command;
        }

        public static Command AddToDateOption(this Command command)
        {
            var toDateOption = new Option<DateTime>(name: "--to" )
            {
                Description = "End date of time window (ISO format: YYYY-MM-DD)",
                Arity = ArgumentArity.ExactlyOne
            };
            command.Add(toDateOption);        
            
            return command;
        }
    }    
}

