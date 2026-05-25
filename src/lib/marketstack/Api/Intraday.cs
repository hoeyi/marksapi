using System;
using System.Threading.Tasks;

namespace ApiClient.Marketstack
{
    # region Endpoint: /intraday
    public partial class MarketstackApi
    {
        /// <summary>
        /// Gets Intraday data for the given symbols and date.
        /// </summary>
        /// <param name="symbols">Symbols to query quotes for.</param>
        /// <param name="dateFrom">Start date of the range to query.</param>
        /// <param name="dateTo">End date of the range to query.</param>
        /// <param name="afterHours">Flag to include after-hours data in query.</param>
        /// <param name="exchangeMic">Filters results for the exchange with the given MIC.</param>
        /// <param name="interval">Timing interval to query. One of {"1min", "5min", "10min", "15min", "30min", "1hour"}.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="IntradayResponse"/>.</returns>
        public async Task<IntradayResponse> GetIntradayResponseAsync(
            string[] symbols, DateTime date, bool afterHours = false, string? exchangeMic = null, string interval = "15min") 
            => await GetIntradayResponseAsync(
                symbols, date, date, afterHours, exchangeMic, interval);

        /// <summary>
        /// Gets Intraday data for the given symbols and date range.
        /// </summary>
        /// <param name="symbols">Symbols to query quotes for.</param>
        /// <param name="dateFrom">Start date of the range to query.</param>
        /// <param name="dateTo">End date of the range to query.</param>
        /// <param name="afterHours">Flag to include after-hours data in query.</param>
        /// <param name="exchangeMic">Filters results for the exchange with the given MIC.</param>
        /// <param name="interval">Timing interval to query. One of {"1min", "5min", "10min", "15min", "30min", "1hour"}.</param>
        /// <returns>A <see cref="Task"/> containing an <see cref="IntradayResponse"/>.</returns>
        public async Task<IntradayResponse> GetIntradayResponseAsync(
            string[] symbols, 
            DateTime dateFrom, 
            DateTime dateTo, 
            bool afterHours = false, 
            string? exchangeMic = null, 
            string interval = "15min")
        {
            var queryBuilder = GetQueryBuilder();
            var symbolsDelimited = string.Join(',', symbols);

            queryBuilder.AddParameter("symbols", symbolsDelimited);
            queryBuilder.AddParameter("date_from", dateFrom.ToString("yyyy-MM-dd"));
            queryBuilder.AddParameter("date_to", dateTo.ToString("yyyy-MM-dd"));
            
            // Optional parameters
            if(afterHours) 
                queryBuilder.AddParameter("after_hours", afterHours.ToString());
            if(!string.IsNullOrEmpty(exchangeMic)) 
                queryBuilder.AddParameter("exchange_mic", exchangeMic);
            
            queryBuilder.AddParameter("interval", interval);

            var response = await GetResponseAsync<IntradayResponse>(queryBuilder, Endpoint.Intraday);

            return response;
        }
    }
    #endregion
}