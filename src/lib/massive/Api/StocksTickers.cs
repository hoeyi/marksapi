using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;

namespace ApiClient.Massive
{
    public partial class MassiveApi
    {

        /// <inheritdoc/>
        public async Task<AggregateTickerResponse> GetStocksAllTickersAsync(
            string? ticker = null, 
            TickerType? type = null,
            string? market = null,
            string? exchange = null,
            string? cusip = null,
            string? cik = null,
            DateTime? date = null,
            string? search = null,
            bool active = true,
            bool asc = true,
            string? sort = null,
            int limit = 100)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(limit, 0, nameof(limit));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(limit, 1000, nameof(limit));
            
            if(string.IsNullOrEmpty(ticker) && string.IsNullOrEmpty(cusip))
                throw new ArgumentException(
                    message: $"One of arguments '{nameof(ticker)}', '{nameof(cusip)}', must be non-empty.");

            // Create query builder and add defined parameters.
            var queryBuilder = GetQueryBuilder();

            // Always given parameters.
            queryBuilder.AddParameter("active", $"{active}");
            queryBuilder.AddParameter("order", asc ? "asc" : "desc");
            queryBuilder.AddParameter("limit", $"{limit}");

            // Conditional parameters.            
            if(!string.IsNullOrEmpty(ticker))
                queryBuilder.AddParameter("ticker", ticker);

            if(type is not null)
                queryBuilder.AddParameter("type", type.Code);

            if(!string.IsNullOrEmpty(market))
                queryBuilder.AddParameter("market", market);

            if(!string.IsNullOrEmpty(exchange))
                queryBuilder.AddParameter("exchange", exchange);

            if(!string.IsNullOrEmpty(cusip))
                queryBuilder.AddParameter("cusip", cusip);

            if(!string.IsNullOrEmpty(cik))
                queryBuilder.AddParameter("cik", cik);

            if(date is not null)
                queryBuilder.AddParameter("date", $"{date:yyyy-MM-dd}");

            if(!string.IsNullOrEmpty(sort))
                queryBuilder.AddParameter("sort", sort);
            
            var response = await GetResponseAsync<AggregateTickerResponse>(queryBuilder, Endpoint.StocksAllTickers);
            
            return response;
        }

        /// <inheritdoc/>
        public async Task<TickerOverviewResponse> GetStocksTickerOverviewResponseAsync(
            string ticker,
            DateTime? date = null
        )
        {
            ArgumentException.ThrowIfNullOrEmpty(ticker);

            string endpointPattern = QueryBuilder
                .ConvertEndpointToStringPattern(Endpoint.StocksTickerOverview);

            string endpoint = string.Format(endpointPattern, ticker);

            var queryBuilder = GetQueryBuilder();
            if(date is not null)
                queryBuilder.AddParameter("date", $"{date:yyyy-MM-dd}");

            var response = await GetResponseAsync<TickerOverviewResponse>(queryBuilder, endpoint);

            return response;
        }

        /// <inheritdoc/>
        public Task<TickerOverviewResponse> GetStocksTickerOverviewResponseAsync(
            string[] ticker, DateTime? date = null)
        {
            throw new NotImplementedException();
        }
    }
}
