using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Marksapi.Cli;

static class ProgramExtensions
{
    /// <summary>
    /// Gets the registered service matching <typeparamref name="T"/>, else
    /// throws and exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns>An instanced of <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T GetServiceOrThrow<T>(this IServiceProvider services)
    {
        return services.GetService<T>() ?? 
            throw new InvalidOperationException(
                message: $"Service '{nameof(T)}' not found.");
    }
}

/// <summary>
/// Simple service provider.
/// </summary>
class SingletonServiceProvider : IServiceProvider
{
    private readonly Dictionary<Type, object> _servies = [];

    /// <summary>
    /// Register a new service.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <returns></returns>
    public SingletonServiceProvider RegisterService<T>(T instance)
    {
        ArgumentNullException.ThrowIfNull(instance, nameof(instance));

        _servies.Add(typeof(T), instance);

        return this;
    }

    public object? GetService(Type serviceType)
    {
        if(!_servies.TryGetValue(serviceType, out object? value))
            return null;
        else
            return value;
    }
}
