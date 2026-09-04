using ApiClient.Massive;
using ApiClient.Massive.Parameters;
using ApiClient.Massive.Response;
using ApiClient.Massive.Response.Economy;
using ApiClient.Massive.Response.Stocks;
using Microsoft.Extensions.Logging;

namespace ApiClient.Test.Massive.Integration
{
    [Trait(nameof(TestAttributeName.Category), "Integration")]
    public class MassiveApi_Test : IClassFixture<IntegrationFixture<MassiveApi>>
    {
        readonly IntegrationFixture<MassiveApi> _fixture;
        ILogger _logger => _fixture.Logger;

        MassiveApi ApiClient => 
            _fixture.MassiveApi ?? 
            throw new InvalidOperationException($"Instance of {nameof(MassiveApi)} required.");
        public MassiveApi_Test(IntegrationFixture<MassiveApi> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void InitializesContext()
        {
            // This method allows generation of logs for inspecting / confirming the 
            // test context was initialized.
            Assert.True(true);
        }
        
        #region aggregate-bar
        [Theory]
        [InlineData("Stocks", "AAPL", 1, "Day", "2025-11-25", "2025-11-28", 5, 3)]
        [InlineData("Options", "SPY260821C00640000", 1, "Day", "2026-06-08", "2026-06-11", 5, 4)]
        [InlineData("Crypto", "BTCUSD", 1, "Day", "2026-06-08", "2026-06-08", 5, 1)]
        [InlineData("Indices", "COMP", 1, "Day", "2026-06-08", "2026-06-08", 5, 1)]
        [InlineData("Fx", "EURUSD", 1, "Day", "2026-06-08", "2026-06-08", 5, 1)]
        [InlineData("Fx", "CADUSD", 1, "Day", "2026-06-08", "2026-06-08", 5, 1)]
        public async Task GetAggregateBarResponseAsync_ReturnSuccessResponse(
            string market, string ticker, int multiplier, string timeSpan, string fromStr, string toStr, int limit, int expectedCount)
        {
            // Arrange
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            
            if(!Enum.TryParse(market, out Market marketResult))
                throw new ArgumentException($"Test parameter '{market}' could not be parsed.");

            if(!Enum.TryParse(timeSpan, out BarTimespan barTimeResult))
                throw new ArgumentException(
                    $"Test parameter '{market}' could not be parsed.");

            // Act
            var responseResult = await ApiClient.GetAggregateBarResponseAsync(
                marketResult, ticker, multiplier, barTimeResult, fromDate, toDate, true, limit);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateBarResponse>(responseResult), 
                () => Assert.Equal(expectedCount, responseResult.CommonCount),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Close > 0))); // verifies complex serialization

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetAggregateBarResponseAsync_ReturnSuccessResponse), 
                responseResult);
        }
        #endregion
        
        #region stocks / tickers
        [Theory]
        [InlineData("AAPL")]
        public async Task GetStocksAllTickerAsync_SingleParameter_Ticker_ReturnSuccessResponse(
            string ticker)
        {
            // Arrange

            // Act
            var responseResult = await ApiClient.GetAllTickersAsync(ticker);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateTickerResponse>(responseResult), 
                () => Assert.Equal(1, responseResult.Count),
                () => Assert.All(responseResult.Results, x => Assert.False(string.IsNullOrEmpty(x?.Name)))); // verifies complex serialization

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetStocksAllTickerAsync_SingleParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("Stocks", "AAPL")]
        [InlineData("Indices", "COMP")]
        [InlineData("Options", "SPY280121C00750000")]
        [InlineData("Crypto", "BTCUSD")]
        [InlineData("Fx", "USDEUR")]
        public async Task GetTickerOverviewResponseAsync_ReturnSuccessResponse(
            string market, string ticker)
        {
            // Arrange
            if(!Enum.TryParse(market, out Market marketResult))
                throw new InvalidOperationException($"Test parameter '{market}' could not be parsed.");

            // Act
            var responseResult = await ApiClient.GetTickerOverviewResponseAsync(marketResult, ticker);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<TickerOverviewResponse>(responseResult),
                () => Assert.NotNull(responseResult.Results)
            );

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetTickerOverviewResponseAsync_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("Stocks", "AAPL")]
        public async Task GetTickerOverviewResponseAsync_ComplexResponse_ReturnSuccessResponse(
            string market, string ticker)
        {
            // Arrange
            Market marketEnum;

            if(!Enum.TryParse(market, out Market result))
                throw new InvalidOperationException($"Test parameter '{market}' could not be parsed.");
            else
                marketEnum = result;

            // Act
            var responseResult = await ApiClient.GetTickerOverviewResponseAsync(marketEnum, ticker);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<TickerOverviewResponse>(responseResult),
                () => Assert.NotNull(responseResult.Results),
                () => Assert.NotNull(responseResult.Results!.Address),
                () => Assert.NotNull(responseResult.Results!.Branding)
            );

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetTickerOverviewResponseAsync_ComplexResponse_ReturnSuccessResponse), 
                responseResult);
        }
        #endregion

        #region short-volume / short-interest
        [Theory]
        [InlineData("AAPL", "2026-05-13", "2026-05-15")]
        public async Task GetShortVolumeResponseAsync_SingleParameter_Ticker_ReturnSuccessResponse(
            string ticker, string fromStr, string toStr)
        {
            // Arrange
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            
            // Act
            var responseResult = await ApiClient.GetShortVolumeResponseAsync(
                [ticker],
                fromDate,
                toDate);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<ShortVolumeResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Date > default(DateTime)))); // verifies complex serialization

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetShortVolumeResponseAsync_SingleParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("AAPL,MSFT", "2026-05-13", "2026-05-15")]
        public async Task GetShortVolumeResponseAsync_MultiParameter_Ticker_ReturnSuccessResponse(
            string ticker, string fromStr, string toStr)
        {
            // Arrange
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            var tickers = ticker.Split(",");

            // Act
            var responseResult = 
                await ApiClient.GetShortVolumeResponseAsync(
                    tickers,
                    fromDate,
                    toDate);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<ShortVolumeResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Date > default(DateTime)))); // verifies complex serialization

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetShortVolumeResponseAsync_MultiParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("AAPL,MSFT", "2026-05-13")]
        public async Task GetShortInterestResponseAsync_MultiParameter_Ticker_ReturnSuccessResponse(
            string ticker, string dateStr)
        {
            // Arrange
            var settleDate = DateTime.Parse(dateStr);
            var tickers = ticker.Split(",");

            // Act
            var responseResult = 
                await ApiClient.GetShortInterestResponseAsync(
                    tickers: tickers,
                    settlementDate: settleDate);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<ShortInterestResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetShortInterestResponseAsync_MultiParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("Gte", "5", "2026-05-13")]
        [InlineData("Lte", "5", "2026-05-13")]
        public async Task GetShortInterestResponseAsync_DaysToCover_SingleParameter_ReturnSuccessResponse(
            string numericOperator, string daysToCover, string dateStr)
        {
            // Arrange
            var settleDate = DateTime.Parse(dateStr);
            var dtcRatio = float.Parse(daysToCover);
            var numOperator = Enum.Parse<NumericComparisonOperator>(numericOperator);

            // Act
            var responseResult = 
                await ApiClient.GetShortInterestResponseAsync(
                        settlementDate: settleDate,
                        daysToCover: new(){{ numOperator, dtcRatio }});
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<ShortInterestResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetShortInterestResponseAsync_DaysToCover_SingleParameter_ReturnSuccessResponse), 
                responseResult);
        }
        
        [Theory]
        [InlineData("Gte,Lte", "1,5", "2026-05-13")]
        public async Task GetShortInterestResponseAsync_DaysToCover_MultiParameter_ReturnSuccessResponse(
            string numericOperator, string daysToCover, string dateStr)
        {
            // Arrange
            var settleDate = DateTime.Parse(dateStr);
            var ratios = daysToCover.Split(",");
            var dtcRatios = ratios.Select(x => float.Parse(x)).ToArray();
            var operators = numericOperator.Split(",");
            var numOperators = operators.Select(x => 
                                    Enum.Parse<NumericComparisonOperator>(x))
                                .ToArray();

            // Act
            var responseResult = 
                await ApiClient.GetShortInterestResponseAsync(
                        settlementDate: settleDate,
                        daysToCover: new(){
                            { numOperators[0], dtcRatios[0] },
                            { numOperators[1], dtcRatios[1]}});
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<ShortInterestResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result            
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetShortInterestResponseAsync_DaysToCover_SingleParameter_ReturnSuccessResponse), 
                responseResult);
        }

        #endregion 

        #region economy
        [Theory]
        [InlineData("2026-06-09")]
        public async Task GetTreasuryYieldResponseAsync_SingleDate_ReturnSuccessResponse(
            string dateStr)
        {
            // Arrange
            var dates = new[] { DateTime.Parse(dateStr) };

            // Act
            var responseResult = await ApiClient.GetTreasuryYieldResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<TreasuryYieldsResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetTreasuryYieldResponseAsync_SingleDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-06-09")]
        public async Task GetTreasuryYieldResponseAsync_MultiDate_ReturnSuccessResponse(
            string datePipeDelim)
        {
            // Arrange
            var dates = datePipeDelim.Split("|").Select(DateTime.Parse).ToArray();

            // Act
            var responseResult = await ApiClient.GetTreasuryYieldResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<TreasuryYieldsResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetTreasuryYieldResponseAsync_SingleDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-06-01")]
        public async Task GetInflationResponseAsync_SingleDate_ReturnSuccessResponse(
            string dateStr)
        {
            // Arrange
            var dates = new[] { DateTime.Parse(dateStr) };

            // Act
            var responseResult = await ApiClient.GetInflationResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<InflationResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetInflationResponseAsync_SingleDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-04-01|2026-05-01")]
        public async Task GetInflationResponseAsync_MultiDate_ReturnSuccessResponse(
            string datePipeDelim)
        {
            // Arrange
            var dates = datePipeDelim.Split("|").Select(DateTime.Parse).ToArray();

            // Act
            var responseResult = await ApiClient.GetInflationResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<InflationResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetInflationResponseAsync_MultiDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-06-01")]
        public async Task GetInflationExpectationResponseAsync_SingleDate_ReturnSuccessResponse(
            string dateStr)
        {
            // Arrange
            var dates = new[] { DateTime.Parse(dateStr) };

            // Act
            var responseResult =
                await ApiClient.GetInflationExpectationResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<InflationExpectationResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetInflationExpectationResponseAsync_SingleDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-05-01|2026-06-01")]
        public async Task GetInflationExpectationResponseAsync_MultiDate_ReturnSuccessResponse(
            string datePipeDelim)
        {
            // Arrange
            var dates = datePipeDelim.Split("|").Select(DateTime.Parse).ToArray();

            // Act
            var responseResult =
                await ApiClient.GetInflationExpectationResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<InflationExpectationResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetInflationExpectationResponseAsync_MultiDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-06-01")]
        public async Task GetLaborMarketResponseAsync_SingleDate_ReturnSuccessResponse(
            string dateStr)
        {
            // Arrange
            var dates = new[] { DateTime.Parse(dateStr) };

            // Act
            var responseResult = await ApiClient.GetLaborMarketResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<LaborMarketResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetLaborMarketResponseAsync_SingleDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-06-05|2026-05-01")]
        public async Task GetLaborMarketResponseAsync_MultiDate_ReturnSuccessResponse(
            string datePipeDelim)
        {
            // Arrange
            var dates = datePipeDelim.Split("|").Select(DateTime.Parse).ToArray();

            // Act
            var responseResult = await ApiClient.GetLaborMarketResponseAsync(dates);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<LaborMarketResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results));

            // Print result
            _logger.LogInformation(
                "'{method}' returned:\n{@responseResult}",
                nameof(GetLaborMarketResponseAsync_MultiDate_ReturnSuccessResponse),
                responseResult);
        }

        [Theory]
        [InlineData("2026-06-08|2026-06-09")]
        public async Task GetTreasuryYieldResponseAsync_NumOpWithMultipleDates_Throws(
            string datePipeDelim)
        {
            // Arrange
            var dates = datePipeDelim.Split("|").Select(DateTime.Parse).ToArray();
            var numOp = NumericComparisonOperator.Gt;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                ApiClient.GetTreasuryYieldResponseAsync(dates, numOp: numOp));
        }
    }
    #endregion
}
