using System;
using System.CommandLine;

namespace Marksapi.Cli;

static class ParseHelper
{
    public static IEnumerable<string> GetDelimitedValues(this string? delimitedString)
    {
        if (!string.IsNullOrWhiteSpace(delimitedString))
        {
            var strArray = delimitedString
                            .Split(
                                separator: ',', 
                                options: StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                            );

            foreach (var t in strArray)
                yield return t;
        }
    }

    public static bool IsValidTimespan(string timespan)
    {
        return timespan.ToLowerInvariant() switch
        {
            "day" or "week" or "month" or "hour" or "minute" => true,
            _ => false
        };
    }

    public static bool InInterval(
        this (double, double) interval,
        double other,
        bool openLeft = true,
        bool openRight = true)
    {
        bool testLeft = openLeft ? 
            other > interval.Item1 : 
            other >= interval.Item1;
        bool testRight = openRight ? 
            other < interval.Item2 : 
            other <= interval.Item2;
        
        return testLeft && testRight;
    }
}
