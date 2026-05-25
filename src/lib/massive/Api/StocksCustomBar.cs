using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using ApiClient.Services;
using ApiClient.Resources;
using System.Collections.Generic;
using ApiClient.Massive.Response;

namespace ApiClient.Massive
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Marketstack API.
    /// </summary>
    public partial class MassiveApi
    {
        public async Task<AggregateBarResponse> GetAggregateBarResponseAsync(
            string ticker, int multiplier, TimeSpan timeSpan, DateTime from, DateTime to)
        {
            var queryBuilder = GetQueryBuilder();

            throw new NotImplementedException();
        }
    }
}
