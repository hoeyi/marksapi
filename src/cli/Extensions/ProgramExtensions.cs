using System;
using System.Diagnostics.CodeAnalysis;
using ApiClient.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ichyd.Marksapi.Cli.Extensions
{
    [ExcludeFromCodeCoverage]
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

        /// <summary>
        /// Getss the <see cref="QueryOptions"/> section of the 'massive' parent element.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>A <see cref="QueryOptions"/> instance if found, else default having range (1, 5000).</returns>
        public static QueryOptions GetQueryOptionsOrDefault(this IConfiguration configuration)
        {   
            QueryOptions options = new();
            var section = configuration
                .GetSection("massive")?
                .GetSection(nameof(QueryOptions));
            if (section is null)
            {
                options.UpperLimit = 5000;
                options.LowerLimit = 1;
            }
            else
                section.Bind(options);

            return options;
        }

        /// <summary>
        /// Gets an <see cref="Interval{int}"/> from this <see cref="QueryOptions"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <returns>An <see cref="Interval{int}"/>.</returns>
        public static Interval<int> QueryLimit(this QueryOptions options) => 
            new(options.LowerLimit, options.UpperLimit, open: false);        
    }
}

