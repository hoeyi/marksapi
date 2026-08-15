using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Ichyd.Marksapi.Cli.Services
{
        /// <summary>
        /// Simple service provider.
        /// </summary>
        [ExcludeFromCodeCoverage]
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
}

