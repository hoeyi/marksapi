using System;
using System.CommandLine;

namespace Marksapi.Cli;

static class ParseHelper
{
    public static string[] ToValueArray(this string? delimitedString)
    {
        if (!string.IsNullOrEmpty(delimitedString))
        {
            var strArray = delimitedString
                            .Split(
                                separator: ',', 
                                options: 
                                    StringSplitOptions.RemoveEmptyEntries | 
                                    StringSplitOptions.TrimEntries
                            );

            return strArray;
        }

        return [delimitedString ?? string.Empty];
    }
}
