using ApiClient.Massive.Parameters;
using ApiClient.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;

namespace ApiClient.Massive
{
    static class QueryBuilderExtensions
    {
        public const string DateFormat = "yyyy-MM-dd";
        
        /// <summary>
        /// Adds the collection of numeric filters as query parameters.
        /// </summary>
        /// <typeparam name="T">Type the parameter being modified accepts.</typeparam>
        /// <param name="queryBuilder">This <see cref="QueryBuilder"/> instance.</param>
        /// <param name="parameterName">The parameter being modified.</param>
        /// <param name="comparisonFilters">The operator, value pairs to append.</param>
        /// <param name="customFormat">Custom format string to apply. Typically used for 
        /// <see cref="DateTime"/> and numeric types.</param>
        public static void AddComparisonFilterParameters<T>(
            this QueryBuilder queryBuilder,
            string parameterName,
            Dictionary<NumericComparisonOperator, T>? comparisonFilters,
            string? customFormat = null)
        where T : IFormattable
        {
            foreach(var kv in comparisonFilters ?? [])
            {
                queryBuilder.AddParameter(
                    $"{parameterName}.{kv.Key.ToString().ToLower()}",
                    kv.Value.ToString(customFormat, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Adds an 'any' parameter for the given name and collection.
        /// </summary>
        /// <param name="queryBuilder">This <see cref="QueryBuilder"/> instance.</param>
        /// <param name="parameterName">The parameter being modified.</param>
        /// <param name="values">The values consisting the inclusion set.</param>
        public static void AddAnyParameter(
            this QueryBuilder queryBuilder,
            string parameterName,
            string[] values) => AddAnyParameter<string>(queryBuilder, parameterName, values);

        /// <summary>
        /// Adds an 'any' parameter for the given name and collection.
        /// </summary>
        /// <typeparam name="T">Type the parameter being modified accepts.</typeparam>
        /// <param name="queryBuilder">This <see cref="QueryBuilder"/> instance.</param>
        /// <param name="parameterName">The parameter being modified.</param>
        /// <param name="values">The values consisting the inclusion set.</param>
        private static void AddAnyParameter<T>(
            this QueryBuilder queryBuilder,
            string parameterName,
            T[] values)
        {
            if(values.Length > 0)
                queryBuilder.AddParameter($"{parameterName}.any_of", string.Join(",", values));
        }
    }
}