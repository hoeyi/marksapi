using ApiClient.Massive;
using ApiClient.Massive.Parameters;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Ichyd.Marksapi.Cli.Massive.Verbs
{
    [ExcludeFromCodeCoverage]
    static class CommandBuilder
    {
        public static Command AddMarketArgument(this Command command)
        {
            var allowedValues = typeof(Market)
                                .GetEnumNames()
                                .Select(x => x.ToLower(CultureInfo.InvariantCulture))
                                .ToArray();

            var marketArgument = new Argument<string>(name: "MARKET")
            {
                Description = $"Target market for query.",
                Arity = ArgumentArity.ExactlyOne,
            };
            marketArgument.AcceptOnlyFromAmong(allowedValues);
            command.Add(marketArgument);
            
            return command;
        }

        public static Command AddTickerArgument(this Command command)
        {
            var tickerArgument = new Argument<string>(name: "TICKER")
            {
                Description = "Target ticker for query. Vary format to match MARKET.",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(tickerArgument);

            return command;
        }

        public static Command AddCikOption(this Command command)
        {
            var cikOption = new Option<string>("--cik")
            {
                Description = "Central Index Key filter.",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(cikOption);

            return command;
        }

        public static Command AddCusipOption(this Command command)
        {
            var cusipOption = new Option<string>("--cusip")
            {
                Description = "CUSIP code filter.",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(cusipOption);

            return command;
        }

        public static Command AddExchangeOption(this Command command)
        {
            var exchangeOption = new Option<string>(name: "--exchange")
            {
                Description = "Primary exchange MIC (ISO 10383).",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(exchangeOption);

            return command;
        }

        public static Command AddActiveOnlyOption(this Command command)
        {
            var inactiveOption = new Option<bool>("--active-only")
            {
                Description = "Exclude inactive tickers.",
                DefaultValueFactory = (args) => true
            };
            command.Add(inactiveOption);

            return command;
        }

        public static Command AddUnadjustedOption(this Command command)
        {
            var unadjustedOption = new Option<bool>("--unadjusted")
            {
                Description = "Return unadjusted quotes.",
                DefaultValueFactory = (args) => false
            };
            command.Add(unadjustedOption);

            return command;
        }

        public static Command AddLimitOption(this Command command)
        {
            var limitOption = new Option<int>(name: "--limit")
            {
                Description = "Maximum records to return.",
                DefaultValueFactory = (args) => 100
            };
            command.Add(limitOption);

            return command;
        }

        public static Command AddMultiplierOption(this Command command)
        {
            var multiplierOption = new Option<int>(name: "--multiplier")
            {
                Description = "Timespan multiplier (e.g., 1 for 1 day, 5 for 5 days).",
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
                Description = "Output format.",
                Arity = ArgumentArity.ZeroOrOne,
                DefaultValueFactory = new((args) => "console")
            };
            formatOption.AcceptOnlyFromAmong("csv", "json", "console");
            command.Add(formatOption);

            return command;
        }

        public static Command AddFileOutputOption(this Command command)
        {
            var formatOption = new Option<string>(name: "--to-file")
            {
                Description = "Directory to write results to.",
                Arity = ArgumentArity.ZeroOrOne
            };
            formatOption.AcceptLegalFilePathsOnly();
            command.Add(formatOption);

            return command;
        }
        
        public static Command AddRatioMinOption(this Command command)
        {
            var ratioMinOption = new Option<float>(name: "--ratio-min")
            {
                Description = "Minimum short volume ratio filter.",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(ratioMinOption);

            return command;
        }

        public static Command AddRatioMaxOption(this Command command)
        {
            var ratioMaxOption = new Option<float>(name: "--ratio-max")
            {
                Description = "Maximum short volume ratio filter.",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(ratioMaxOption);

            return command;
        }

        public static Command AddSearchOption(this Command command)
        {
            var searchOption = new Option<string>("--search")
            {
                Description = "Search within ticker/company name.",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(searchOption);

            return command;
        }

        public static Command AddSortDescendingOption(this Command command)
        {
            var sortOption = new Option<bool>(name: "--desc")
                {
                    Description = "Sort descending.",
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
                    Description = "Field to sort results by.",
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

        public static Command AddTickersOptionRequired(this Command command)
        {
            var tickerOption = new Option<string>(name: "--tickers")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "Multiple comma-separated ticker symbols."
            };
            command.Add(tickerOption);

            return command;
        }

        public static Command AddTickersOption(this Command command)
        {
            var tickerOption = new Option<string>(name: "--tickers")
            {
                Arity = ArgumentArity.ZeroOrOne,
                Description = "Multiple comma-separated ticker symbols."
            };
            command.Add(tickerOption);

            return command;
        }

        public static Command AddTickerTypeOption(this Command command)
        {
            var typeOption = new Option<string>(name: "--type")
            {
                Description = "Filter by ticker type."
            };
            command.Add(typeOption);

            return command;
        }

        public static Command AddTimespanOption(this Command command)
        {
            var timespanOption = new Option<string>(name: "--timespan")
            {
                Description = "Time window size.",
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
                Description = "Snapshot date (ISO format: YYYY-MM-DD).",
                Arity = ArgumentArity.ZeroOrOne
            };
            command.Add(dateOption);

            return command;
        }
        
        public static Command AddDateArrayOption(
            this Command command, ArgumentArity? arity = null)
        {
            var dateOption = new Option<DateTime[]>(name: "--date")
            {
                Description = "Snapshot date(s) (ISO format: YYYY-MM-DD).",
                Arity = arity ?? ArgumentArity.OneOrMore,
                AllowMultipleArgumentsPerToken = true
            };
            command.Add(dateOption);

            return command;
        }

        public static Command AddFromDateOption(this Command command)
        {
            var fromDateOption = new Option<DateTime>(name: "--from" )
            {
                Description = "Start date of time window (ISO format: YYYY-MM-DD).",
                Arity = ArgumentArity.ExactlyOne
            };
            command.Add(fromDateOption);

            return command;
        }

        public static Command AddToDateOption(this Command command)
        {
            var toDateOption = new Option<DateTime>(name: "--to" )
            {
                Description = "End date of time window (ISO format: YYYY-MM-DD).",
                Arity = ArgumentArity.ExactlyOne
            };
            command.Add(toDateOption);        
            
            return command;
        }

        public static Command AddComparisonArrayOption(this Command command)
        {
            static NumericComparisonOperator ConvertOrThrow(string s)
            {
                var culture = CultureInfo.InvariantCulture;
                var strMember = culture.TextInfo.ToTitleCase(s.ToLower());

                if(Enum.TryParse(strMember, out NumericComparisonOperator enumMember))
                    return enumMember;
                else
                    throw new ArgumentException(
                        $"Could not convert '{s}' to {typeof(NumericComparisonOperator).Name} member.");
            }

            var option = new Option<NumericComparisonOperator[]>(name: $"--operator" )
            {
                Description = "Comparison operator.",
                Arity = ArgumentArity.ZeroOrMore,
                CustomParser = result =>
                {
                    if(result.Tokens.Count > 0)
                        return result.Tokens.Select(x => ConvertOrThrow(x.Value)).ToArray();
                    else
                        return null;
                }
            };
            var names = Enum
                        .GetValues<NumericComparisonOperator>()
                        .Select(x => x.ToString().ToLowerInvariant())
                        .ToArray();
            option.AcceptOnlyFromAmong(names);

            command.Add(option);
            return command;
        }

        public static Command AddComparisonOptions(this Command command)
        {
            var option = new Option<string[]>(name: $"--operator" )
            {
                Description = "Comparison operator.",
                Arity = ArgumentArity.ZeroOrMore
            };
            var names = Enum
                        .GetValues<NumericComparisonOperator>()
                        .Select(x => x.ToString().ToLowerInvariant())
                        .ToArray();
            option.AcceptOnlyFromAmong(names);

            command.Add(option);

            return command;
        }

        public static Command AddDaysToCoverOptions(this Command command)
        {
            var option = new Option<float[]>(name: "--days-to-cover")
            {
                Description = "Days to cover ratio to limit results (pair with --operator).",
                Arity = ArgumentArity.ZeroOrMore
            };

            command.Add(option);

            return command;
        }

        public static Command AddAverageDailyVolumeOptions(this Command command)
        {
            var option = new Option<float[]>(name: "--avg-volume")
            {
                Description = "Average daily volume to limit results (pair with --operator).",
                Arity = ArgumentArity.ZeroOrMore
            };

            command.Add(option);

            return command;
        }

        public static Command AddShortDailyVolumeOptions(this Command command)
        {
            var option = new Option<float[]>(name: "--short-volume-ratio")
            {
                Description = "Short daily volume to limit results (pair with --operator)",
                Arity = ArgumentArity.ZeroOrMore
            };

            command.Add(option);

            return command;
        }

        public static Command AddSettlementDateOption(this Command command)
        {
            var option = new Option<DateTime>(name: "--settlement")
            {
                Description = "Date as of which data is settled (ISO format: YYYY-MM-DD)",
                Arity = ArgumentArity.ZeroOrOne
            };
            
            command.Add(option);

            return command;
        }

        public static Dictionary<string, float>? ConvertNumericArguments(
            float[] @values,
            string[] @operators,
            int offset = 0
        )
        {
            var validator = new CommandValidator(logger: null);
            // If all input arrays are zero, there are no numeric arguments to append.
            if(@values.Length == @operators.Length - offset & @operators.Length - offset == 0)
                return null;
                
            if(values.Length > operators.Length - offset)
                throw new InvalidOperationException(
                    $"Unexpected argument lengths. Parameter '{nameof(@values)}' must " +
                    $"must have equal or lesser length than '{nameof(@operators)}'.");

            var dict = new Dictionary<string, float>();
            for(int i = 0; i < values.Length; i++)
            {
                dict.Add(@operators[i+offset], values[i]);
            }
            return dict;
        }

        public static Dictionary<NumericComparisonOperator, T>? ConvertNumericArguments<T>(
            T[]? @values,
            NumericComparisonOperator[]? @operators,
            int offset = 0
        )
        {
            if(@values is null || @operators is null)
                return null;
                
            // If all input arrays are zero, there are no numeric arguments to append.
            if(@values.Length == @operators.Length - offset & @operators.Length - offset == 0)
                return null;
                
            if(values.Length > operators.Length - offset)
                throw new InvalidOperationException(
                    $"Unexpected argument lengths. Parameter '{nameof(@values)}' must " +
                    $"must have equal or lesser length than '{nameof(@operators)}'.");

            var dict = new Dictionary<NumericComparisonOperator, T>();
            for(int i = 0; i < values.Length; i++)
            {
                dict.Add(@operators[i+offset], values[i]);
            }
            return dict;
        }        
    }    
}

