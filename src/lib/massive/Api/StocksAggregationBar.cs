using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiClient.Massive.Response;

namespace ApiClient.Massive
{
    /// <summary>
    /// Service class for handling sending and receiving requests to Marketstack API.
    /// </summary>
    public partial class MassiveApi
    {
        private string _endPointFormat = Endpoint.StocksCustomBars;
        public async Task<AggregateBarResponse> GetAggregateBarResponseAsync(
            string ticker, int multiplier, BarTimespan timeSpan, DateTime from, DateTime to)
        {
            var queryBuilder = GetQueryBuilder();

            
            throw new NotImplementedException();
        }
    }
}
