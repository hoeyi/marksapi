using ApiClient.Massive.Parameters;
using ApiClient.Massive.Response;
using ApiClient.Massive.Response.Economy;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiClient.Massive;

static class QueryBuilderExtensions
{
    /// <summary>
    /// Adds the 'date' parameter to queries leveraging numeric operator comparisons.
    /// </summary>
    /// <param name="queryBuilder"></param>
    /// <param name="dates">The dates to add to the query.</param>
    /// <param name="numOp">The numeric operator to apply. </param>
    public static void AddDateParameterWithComparison(
        this QueryBuilder queryBuilder,
        DateTime[] dates,
        NumericComparisonOperator? numOp)
    {
        bool numOpNull = numOp is null;

        if(numOpNull && dates.Length > 1)
            throw new ArgumentException(
                $"Parameter '{nameof(dates)}' expects length 1 if '{nameof(numOp)}' provided.");

        var dateDelimStr = string.Join(
            ",",
            dates.Select(x => DateOnly.FromDateTime(x).ToString("O")));
        
        if(numOpNull)
            queryBuilder.AddParameter("date.any_of", dateDelimStr);
        else
            queryBuilder.AddParameter($"date.{numOp}", dateDelimStr);
    }
}