using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace ApiClient.Marketstack.Services
{
    /// <summary>
    /// Helper class for constructing parameters to pass as a URI query.
    /// </summary>        
    public class QueryBuilder
    {
        private readonly Dictionary<string, string> _params = [];

        /// <summary>
        /// Creates a new instance of <see cref="QueryBuilder"/>.
        /// </summary>
        public QueryBuilder()
        {
            Parameters = new ReadOnlyDictionary<string, string>(_params);
        }

        /// <summary>
        /// Creates a new instance of <see cref="QueryBuilder"/>.
        /// </summary>
        /// <param name="requiredParameters"></param>
        /// <exception cref="ArgumentException">Too many required parameters provided.</exception>
        public QueryBuilder(params KeyValuePair<string, string>[] requiredParameters)
        {
            if(requiredParameters.Length > 5) 
                throw new ArgumentException(message: "Required parameters cannot be longer than 5");

            foreach(var kv in requiredParameters)
            {
                AddParameter(key: kv.Key, value: kv.Value);
            }

            Parameters = new ReadOnlyDictionary<string, string>(_params);
        }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Adds the given key and <typeparamref name="T"/> value to the query.
        /// </summary>
        /// <typeparam name="T">The input type for the parameter value.</typeparam>
        /// <param name="key">The parameter name / key.</param>
        /// <param name="value">The parameter value.</param>
        /// <param name="format"><em>optional</em>: the format for the parameter value.</param>
        public void  AddParameter<T>(string key, T value, string? format = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            ArgumentNullException.ThrowIfNull(value);
        
            if(string.IsNullOrEmpty(format)) _params.Add(key.ToLower(), $"{value}".ToLower());
            else _params.Add(key.ToLower(), $"{value}:{format}".ToLower());
        }

        /// <summary>
        /// Removes the given parameter if it exists.
        /// </summary>
        /// <param name="key">The key of the parameter to remove.</param>
        public void RemoveParameter(string key)
        {
            if(_params.Keys.Contains(key)) _params.Remove(key);
        }

        /// <summary>
        /// Returns the query string.
        /// </summary>
        public override string ToString()
        {
            var queryString = new StringBuilder("?");

            queryString.Append(string.Join("&", _params.Select(kv => $"{kv.Key}={kv.Value}")));

            return queryString.ToString();
        }
    }
}