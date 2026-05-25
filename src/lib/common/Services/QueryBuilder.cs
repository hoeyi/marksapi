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
        private readonly short _maximumDateRangeInDays = 30;
        
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

        /// <summary>
        /// Converts a parameterized endpoint to a pattern suitable for position string interpolation.
        /// </summary>
        /// <param name="endpoint">An variable endpoint.</param>
        /// <returns>A new string with parameters with positional placeholders.</returns>
        /// <remarks>Examples:<list type="bullet"><item>/api/{resource}/detail/{date} => /api/{0}/detail/{1}</item></list></remarks>
        public static string ConvertEndpointToStringPattern(string endpoint)
        {
            string pattern = @"\{[^}]*\}";
            Regex r = new(pattern, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(endpoint);

            StringBuilder endpointBuilder = new(endpoint);

            // Traverse in reverse order to keep the lower-index matches
            // at the same index.
            for(int i = mc.Count - 1; i >= 0; i--)
            {
                Match m = mc[i];
                endpointBuilder.Remove(m.Index, m.Length);
                endpointBuilder.Insert(m.Index, $"{{{i}}}");

                Debug.WriteLine($"Match:\n\t(index = {m.Index}, length = {m.Length})");
            }

            return endpointBuilder.ToString();
        }

        /// <summary>
        /// Validates the given dates form an acceptable date range parameter.
        /// </summary>
        /// <param name="dateFrom">Start date of the range tested.</param>
        /// <param name="dateTo">End date of the range tested.</param>
        /// <returns>Return <see cref="True"/> if the range is acceptable, else throw <see cref="ArgumentException"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="dateFrom"/> is greater than <paramref name="dateTo"/> or the 
        /// range measured in days is too long.</exception>
        public bool ValidateDateRangeOrThrow(DateTime dateFrom, DateTime dateTo)
        {
            if(dateFrom > dateTo)
            {
                throw new ArgumentException(
                    $"Range invalid: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}");
            }
            if(dateTo.Subtract(dateFrom).Days > _maximumDateRangeInDays)
            {
                throw new ArgumentException(
                    $"Range too long: [{nameof(dateFrom)} = {dateFrom:d}, {nameof(dateTo)} = {dateTo:d}");
            }
            return true;
        }
    }
}