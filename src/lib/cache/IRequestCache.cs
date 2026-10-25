using ApiClient.Cache.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiClient.Cache
{
    /// <summary>
    /// Manages cached results for structured requests. Entries persist until flushed or the source is removed.
    /// </summary>
    public interface IRequestCache
    {
        /// <summary>
        /// Indicates whether a result is cached for the request.
        /// </summary>
        Task<bool> ContainsAsync(StructuredRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Removes every entry held by this cache.
        /// </summary>
        Task FlushAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the cached result for a matching request; a failed <see cref="Result{T}"/> indicates a miss.
        /// </summary>
        Task<StructuredRequest> GetAsync(object id, CancellationToken cancellationToken);

        /// <summary>
        /// Stores or overwrites the result for the request.
        /// </summary>
        Task<int> SetAsync(StructuredRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Manages cached results for structured requests. Entries persist until flushed or the source is removed.
    /// </summary>
    /// <typeparam name="TResult">The cached result type.</typeparam>
    public interface IRequestCache<TResult>
        where TResult : class
    {
        /// <summary>
        /// Indicates whether a result is cached for the request.
        /// </summary>
        Task<bool> ContainsAsync(StructuredRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Removes every entry held by this cache.
        /// </summary>
        Task FlushAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns the cached result for a matching request; a failed <see cref="Result{T}"/> indicates a miss.
        /// </summary>
        Task<TResult> GetAsync(StructuredRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Stores or overwrites the result for the request.
        /// </summary>
        Task<int> SetAsync(StructuredRequest request, TResult result, CancellationToken cancellationToken);
    }
}