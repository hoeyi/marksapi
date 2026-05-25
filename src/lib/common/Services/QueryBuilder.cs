using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace ApiClient.Services
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
        public QueryBuilder() : this(initParameters: [])
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="QueryBuilder"/>.
        /// </summary>
        /// <param name="initParameters">The initial parameter key-value pairs to include.</param>
        public QueryBuilder(params KeyValuePair<string, string>[] initParameters)
        {
            foreach(var kv in initParameters ?? [])
                AddParameter(key: kv.Key, value: kv.Value);

            Parameters = new ReadOnlyDictionary<string, string>(_params);
        }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Adds the given key and value to the query.
        /// </summary>
        /// <typeparam name="T">The input type for the parameter value.</typeparam>
        /// <param name="key">The parameter name / key.</param>
        /// <param name="value">The parameter value.</param>
        /// <param name="format"><em>optional</em>: the format for the parameter value.</param>
        /// <exception cref="ArgumentException"><paramref name="key"/> was null, empty or whitespace.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> was null.</exception>
        public void  AddParameter(string key, string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);
            ArgumentException.ThrowIfNullOrEmpty(key);
            ArgumentNullException.ThrowIfNull(value);

            _params.Add(key.ToLower(), value);
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

        public static string ConvertEndpointToStringPattern(string endpoint)
        {
            string pattern = "{(.*?)}";
            Regex r = new(pattern, RegexOptions.IgnoreCase);

            Match m = r.Match(endpoint);
            int matchCount = 0;
            foreach(Group g in m.Groups)
            {
                matchCount += 1;
                for(int j = 0; j < g.Captures.Count; j++)
                {
                    Console.WriteLine($"Index: {g.Captures[j].Index}; Length: {g.Captures[j].Length}");
                }
            }

            return string.Empty;
        }
    }
}