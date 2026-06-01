using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ApiClient.Services
{
    /// <summary>
    /// Abstraction of common features for API clients.
    /// </summary>
    public abstract class ApiClient
    {
        /// <summary>
        /// The base hostname / URL for the API.
        /// </summary>
        private readonly string _baseUrl = default!;

        /// <summary>
        /// The <see cref="HttpClient"/> used for making calls.
        /// </summary>
        private HttpClient _httpClient = default!;

        /// <summary>
        /// Collection of requirement parameters and values to append to every Http call.
        /// </summary>
        protected Dictionary<string, string> RequiredParams { get; init; } = [];

        /// <summary>
        /// Gets the base <see cref="Uri"/>  for this client.
        /// </summary>
        protected Uri BaseUri { get; } = default!;

        /// <summary>
        /// Gets the string representation of the base URL for this client.
        /// </summary>
        protected string BaseUrl { get; } = default!;

        /// <summary>
        /// Gets the <see cref="System.Net.Http.HttpClient"/> for this client.
        /// </summary>
        protected HttpClient HttpClient => _httpClient;

        /// <summary>
        /// Initializes the base API client functionality with an empty required parameter
        /// set.
        /// </summary>
        /// <param name="baseUrl">The base URL for the API.</param>
        /// <param name="httpClient">The <see cref="System.Net.Http.HttpClient"/> for this API client.</param>
        protected ApiClient(
            string baseUrl, 
            HttpClient httpClient)
        {
            ArgumentException.ThrowIfNullOrEmpty(baseUrl);

            _baseUrl = baseUrl;
            _httpClient = httpClient;

            BaseUri = new(_baseUrl);                
        }
    }
}