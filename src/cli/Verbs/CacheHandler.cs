using ApiClient.Cache;
using Ichyd.Marksapi.Cli.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Ichyd.Marksapi.Cli.Verbs;

[ExcludeFromCodeCoverage]
public class CacheHandler
{
    public static Command CreateCommand()
    {
        var command = new Command("cache", "Query or flush the request cache")
        {
            new Option<string>("--contains")
            {
                Description = "The id or command to search",
                Arity = ArgumentArity.ZeroOrOne
            },
            new Option<string>("--get")
            {
                Description = "Get a specific id",
                Arity = ArgumentArity.ZeroOrOne
            },
            new Option<string>("--flush")
            {
                Description = "Flush the request cache",
                Arity = ArgumentArity.ZeroOrOne
            }
        };

        return command;
    }

    /// <inheritdoc/>
    private static void Handle(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger>();
        var requestCache = serviceProvider.GetRequiredService<IRequestCache>();
    }    
}
