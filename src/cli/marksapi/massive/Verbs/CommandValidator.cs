using System;
using System.CommandLine;
using ApiClient.Massive;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;
using Microsoft.Extensions.Logging;

namespace Marksapi.Cli.Massive.Verbs
{
    /// <summary>
    /// Handles argument validation for common command arguments and options.
    /// </summary>
    /// <param name="logger"></param>
    class CommandValidator
    {
        private readonly ILogger? _logger;
        public CommandValidator(ILogger? logger = null) => _logger = logger;
        
        public CommandValidator ValidateDateRangeOrThrow(DateTime? fromDate, DateTime? toDate)
        {
            if(!(fromDate.HasValue && toDate.HasValue))
            {
                _logger?.LogError(
                    "Both options <{opt1}> and <{opt2}> must be specified.", "--from", "--to");
                throw new ArgumentException(
                    $"Parameters: {nameof(fromDate)}, {nameof(toDate)}.");
            }
            
            return this;
        }

        public CommandValidator ValidateFormatOrThrow(string? format)
        {
            string[] supported = ["csv", "json"];
            if(!string.IsNullOrEmpty(format) && !supported.Contains(format))
            {
                _logger?.LogError(
                    "Option <{opt}> must be one of: {supported}.", 
                    "--format", 
                    string.Join(", ", supported));
                throw new ArgumentException($"Parameters: {nameof(format)}.");
            }

            return this;            
        }

        public CommandValidator ValidateLimitOrThrow<T>(
            T? limit,
            Interval<T> interval
        )
            where T : struct, IEquatable<T>, IComparable<T>
        {
            if(limit.HasValue)
            {
                if (!interval.Contains(limit.Value))
                {
                    _logger?.LogError(
                        "Option <{opt}> must be between {lower} and {upper}.", 
                        "--limit", 
                        Program.QueryLimit.Start,
                        Program.QueryLimit.End);
                    throw new ArgumentException(
                        $"Parameter '{nameof(limit)}' out of range.");
                }
            }

            return this;
        }

        public CommandValidator ValidateMarketOrThrow(
            string? market,
            out Market mktEnum)
        {
            if(!Enum.TryParse(market, out Market result))
            {
                _logger?.LogError(
                    "Argument <{arg}> must be one of: crypto, fx, indices, options, stocks.",
                    "MARKET");
                throw new ArgumentException(
                    $"Invalid parameters: {nameof(market)}.");
            }
            mktEnum = result;
            return this;
        }

        public CommandValidator ValidateRatioRangeOrThrow(float? ratioMin, float? ratioMax)
        {
            if (ratioMin.HasValue && ratioMax.HasValue && ratioMin > ratioMax)
            {
                _logger?.LogError(
                    "Option <{opt1}> must be less than <{opt2}>.",
                    "--ratio-min",
                    "--ratio-max");
                throw new ArgumentException(
                    $"Invalid parameters: {nameof(ratioMin)}, {nameof(ratioMax)}.");
            }

            return this;
        }

        public CommandValidator ValidateTickerOrThrow(string? ticker)
        {
            if(string.IsNullOrEmpty(ticker) || string.IsNullOrWhiteSpace(ticker))
            {
                _logger?.LogError(
                    "Argument <{arg}> must be specified.", 
                    "TICKER");
                throw new ArgumentException(
                    $"Parameters: {nameof(ticker)}.");
            }

            return this;            
        }

        public CommandValidator ValidateTickerOrTickersOrThrow(string? ticker, string? tickers)
        {
            if(
                (string.IsNullOrEmpty(ticker) & string.IsNullOrEmpty(tickers)) ||
                (string.IsNullOrWhiteSpace(ticker) & string.IsNullOrWhiteSpace(tickers)))
            {
                _logger?.LogError(
                    "Either argument <{arg}> or option <{opt}> must be specified.", 
                    "TICKER", 
                    "--ticker");
                throw new ArgumentException(
                    $"Either argument {nameof(ticker)} or {nameof(tickers)} must be specified.");
            }

            return this;            
        }

        public CommandValidator ValidateTimespanOrThrow(
            string? timespan,
            out BarTimespan barTimespan)
        {
            if(!Enum.TryParse(timespan, out BarTimespan result))
            {
                _logger?.LogError(
                    "Option <{opt}> must be one of: second, minute, hour, day, week, month, quarter, year.",
                    "--timespan");
                throw new ArgumentException(
                    $"Invalid parameters: {nameof(timespan)}.");
            }
            barTimespan = result;
            return this;
        }
    }
}

