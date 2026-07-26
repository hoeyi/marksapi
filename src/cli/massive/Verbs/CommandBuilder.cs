using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace Ichyd.Marksapi.Cli.Massive.Verbs
{
    [ExcludeFromCodeCoverage]
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

        public static Command AddCikOption(this Command command)
        {
            var cikOption = new Option<string>("--cik")
            {
                Description = "Central Index Key filter",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(cikOption);

            return command;
        }

        public static Command AddCusipOption(this Command command)
        {
            var cusipOption = new Option<string>("--cusip")
            {
                Description = "CUSIP code filter",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(cusipOption);

            return command;
        }

        public static Command AddExchangeOption(this Command command)
        {
            var exchangeOption = new Option<string>(name: "--exchange")
            {
                Description = "Primary exchange MIC (ISO 10383)",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(exchangeOption);

            return command;
        }

        public static Command AddActiveOnlyOption(this Command command)
        {
            var inactiveOption = new Option<bool>("--active-only")
            {
                Description = "Exclude inactive tickers. Default is true.",
                DefaultValueFactory = (args) => true
            };
            command.Add(inactiveOption);

            return command;
        }

        public static Command AddLimitOption(this Command command)
        {
            var limitOption = new Option<int>(name: "--limit")
            {
                Description = "Maximum records to return. Default is 100.",
                DefaultValueFactory = (args) => 100
            };
            command.Add(limitOption);

            return command;
        }

        public static Command AddMultiplierOption(this Command command)
        {
            var multiplierOption = new Option<int>(name: "--multiplier")
            {
                Description = "Timespan multiplier (e.g., 1 for 1 day, 5 for 5 days). Default is 1.",
                Arity = ArgumentArity.ExactlyOne,
                DefaultValueFactory = (args) => 1
            };
            command.Add(multiplierOption);

            return command;
        }

        public static Command AddFormatOption(this Command command)
        {
            var formatOption = new Option<string>(name: "--format")
            {
                Description = "Output format (json, csv, console)",
                Arity = ArgumentArity.ZeroOrOne,
                DefaultValueFactory = new((args) => "console")
            };
            command.Add(formatOption);

            return command;
        }

        public static Command AddFileOutputOption(this Command command)
        {
            var formatOption = new Option<string>(name: "--to-file")
            {
                Description = "Directory to write results to",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(formatOption);

            return command;
        }
        
        public static Command AddRatioMinOption(this Command command)
        {
            var ratioMinOption = new Option<float>(name: "--ratio-min")
            {
                Description = "Minimum short volume ratio filter",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(ratioMinOption);

            return command;
        }

        public static Command AddRatioMaxOption(this Command command)
        {
            var ratioMaxOption = new Option<float>(name: "--ratio-max")
            {
                Description = "Maximum short volume ratio filter",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(ratioMaxOption);

            return command;
        }

        public static Command AddSearchOption(this Command command)
        {
            var searchOption = new Option<string>("--search")
            {
                Description = "Search within ticker/company name",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(searchOption);

            return command;
        }

        public static Command AddSortDescendingOption(this Command command)
        {
            var sortOption = new Option<bool>(name: "--desc")
                {
                    Description = "Sort descending",
                    Arity = ArgumentArity.ZeroOrOne,
                    DefaultValueFactory = new((args) => false)
                };
                command.Add(sortOption);

            return command;
        }

        public static Command AddSortFieldOption(this Command command)
        {
            var sortOption = new Option<string>(name: "--sort")
                {
                    Description = "Field to sort results by",
                    Arity = ArgumentArity.ZeroOrOne
                };
                command.Add(sortOption);

            return command;
        }

        public static Command AddTickerOption(this Command command)
        {
            var tickerArgument = new Option<string>(name: "--ticker")
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
                Arity = ArgumentArity.ZeroOrOne,
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
                Description = "Time window size (second, minute, hour, day, week, month, quarter, year).\nDefault is 'day'.",
                Arity = ArgumentArity.ExactlyOne,
                DefaultValueFactory = (args) => "day"
            };
            command.Add(timespanOption);

            return command;
        }

        public static Command AddDateOption(this Command command)
        {
            var dateOption = new Option<DateTime>(name: "--date")
            {
                Description = "Snapshot date (YYYY-MM-DD)",
                Arity = ArgumentArity.ZeroOrOne
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

