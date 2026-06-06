using System;
using System.Threading.Tasks;
using ApiClient.Massive.Response;
using ApiClient.Massive.Response.Stocks;
using ApiClient.Services;

namespace ApiClient.Massive
{
    public partial class MassiveApi
    {

        /// <summary>
        /// Submits queries to the endpoint <em>/v3/reference/tickers</em>.
        /// </summary>
        /// <param name="ticker">Filter by a ticker symbol. Defaults to empty string which queries all tickers.</param>
        /// <param name="type">Filter by the type of the tickers. Defaults to empty string which queries all types.</param>
        /// <param name="market">Filter by market type. By default all markets are included.</param>
        /// <param name="exchange">Filter by the asset's primary exchange Market Identifier Code (MIC) according to ISO 10383. Defaults to empty string which queries all exchanges.</param>
        /// <param name="cusip">Filter by the CUSIP code of the asset you want to search for.</param>
        /// <param name="cik">Filter by the Central Index Key of the asset.</param>
        /// <param name="date">Specify a point in time to retrieve tickers available on that date. Defaults to the most recent available date.</param>
        /// <param name="search">Filter for terms within the ticker and/or company name.</param>
        /// <param name="active">Filter for active tickers only.</param>
        /// <param name="asc">Sort the results by ascending order.</param>
        /// <param name="sort">The field to sort by.</param>
        /// <param name="limit">Limit the number of results returned, default is 100 and max is 1000.</param>
        /// <returns>A <see cref="Task"/> containing a <see cref="AggregateTickerResponse"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="limit"/> was not in the interval (0,1000].</exception>
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

        /// <summary>
        /// Retrieve comprehensive details for a single ticker supported by Massive that is active as-of a given date
        /// </summary>
        /// <param name="ticker"></param>
        /// <param name="date"></param>
        /// <returns></returns>
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
    }
}
