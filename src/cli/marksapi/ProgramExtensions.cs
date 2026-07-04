using System;
using Microsoft.Extensions.DependencyInjection;

namespace Marksapi.Cli;

public static class ProgramExtensions
{
    /// <summary>
    /// Gets the registered service matching <typeparamref name="T"/>, else
    /// throws and exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns>An instanced of <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T GetServiceOrThrow<T>(this IServiceProvider services) => 
        services.GetService<T>() ?? 
            throw new InvalidOperationException(
                message: $"Service '{nameof(T)}' not found.");
}
