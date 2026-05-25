using System;
using System.Threading.Tasks;

namespace ApiClient.Marketstack
{
    # region Endpoint: /eod
    public partial class MarketstackApi
    {
        /// <summary>
        /// Gets Eod data for the given symbols and date range.
        /// </summary>
        /// <param name="symbols">Array of stock or bond tickers.</param>
        /// <param name="dateFrom">Start date of the query range.</param>
        /// <param name="dateTo">End date of the query range.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="EodResponse"/>.</returns>
        public async Task<EodResponse> GetEodResponseAsync(
                    string[] symbols, DateTime dateFrom, DateTime dateTo)
        {
            

            var queryBuilder = GetQueryBuilder();
            var symbolsDelimited = string.Join(',', symbols);

            queryBuilder.AddParameter("symbols", symbolsDelimited);
            queryBuilder.AddParameter("date_from", dateFrom.ToString("yyyy-MM-dd"));
            queryBuilder.AddParameter("date_to", dateTo.ToString("yyyy-MM-dd"));

            var response = await GetResponseAsync<EodResponse>(queryBuilder, Endpoint.Eod);

            return response;
        }

        /// <summary>
        /// Gets Eod data for the given symbols and date.
        /// </summary>
        /// <param name="symbols">Array of stock or bond tickers.</param>
        /// <param name="date">Date to fetch data for.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="EodResponse"/>.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<EodResponse> GetEodResponseAsync(string[] symbols, DateTime date)
            => await GetEodResponseAsync(symbols: symbols, dateFrom: date, dateTo: date);
    }
    #endregion
}