using ApiClient.Massive;
using ApiClient.Massive.Response.Stocks;
using Microsoft.Extensions.Logging;

namespace ApiClient.Test.Massive.Integration
{
    [Trait(nameof(TestAttributeName.Category), "Integration")]
    public class MassiveApi_Test : IClassFixture<ConfigurationFixture>
    {
        ConfigurationFixture _fixture;
        public MassiveApi_Test(ConfigurationFixture fixture)
        {
            _fixture = fixture;
            ArgumentException.ThrowIfNullOrWhiteSpace(_fixture.Configuration["api_key:massive"]);
        }

        [Theory]
        [InlineData("Stocks", "AAPL", 1, "Day", "2025-11-25", "2025-11-28", 5, 3)]
        [InlineData("Options", "SPY260821C00640000", 1, "Day", "2026-06-08", "2026-06-11", 5, 4)]
        [InlineData("Crypto", "BTCUSD", 1, "Day", "2026-06-08", "2026-06-08", 5, 1)]
        [InlineData("Indices", "COMP", 1, "Day", "2026-06-08", "2026-06-08", 5, 1)]
        [InlineData("FX", "EURUSD", 1, "Day", "2026-06-08", "2026-06-08", 5, 1)]
        public async Task GetAggregateBarResponseAsync_ReturnSuccessResponse(
            string market, string ticker, int multiplier, string timeSpan, string fromStr, string toStr, int limit, int expectedCount)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            
            if(!Enum.TryParse(market, out Market marketResult))
                throw new ArgumentException($"Test parameter '{market}' could not be parsed.");

            if(!Enum.TryParse(timeSpan, out BarTimespanEnum barTimeResult))
                throw new ArgumentException(
                    $"Test parameter '{market}' could not be parsed.");

            // Act
            var responseResult = await apiClient.GetAggregateBarResponseAsync(
                marketResult, ticker, multiplier, barTimeResult, fromDate, toDate, limit);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateBarResponse>(responseResult), 
                () => Assert.Equal(expectedCount, responseResult.ResultsCount),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Close > 0))); // verifies complex serialization

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetAggregateBarResponseAsync_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("AAPL")]
        public async Task GetStocksAllTickerAsync_SingleParameter_Ticker_ReturnSuccessResponse(
            string ticker)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);

            // Act
            var responseResult = await apiClient.GetAllTickersAsync(ticker);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateTickerResponse>(responseResult), 
                () => Assert.Equal(1, responseResult.Count),
                () => Assert.All(responseResult.Results, x => Assert.False(string.IsNullOrEmpty(x?.Name)))); // verifies complex serialization

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetStocksAllTickerAsync_SingleParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("Stocks", "AAPL")]
        [InlineData("Indices", "COMP")]
        [InlineData("Options", "SPY260821C00640000")]
        [InlineData("Crypto", "BTCUSD")]
        [InlineData("FX", "USDEUR")]
        public async Task GetTickerOverviewResponseAsync_ReturnSuccessResponse(
            string market, string ticker)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!, _fixture.Configuration);

            if(!Enum.TryParse(market, out Market marketResult))
                throw new InvalidOperationException($"Test parameter '{market}' could not be parsed.");

            // Act
            var responseResult = await apiClient.GetTickerOverviewResponseAsync(marketResult, ticker);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<TickerOverviewResponse>(responseResult),
                () => Assert.NotNull(responseResult.Results)
            );

            // Print result            
            _fixture.Logger.LogInformation(
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
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!, _fixture.Configuration);
            Market marketEnum;

            if(!Enum.TryParse(market, out Market result))
                throw new InvalidOperationException($"Test parameter '{market}' could not be parsed.");
            else
                marketEnum = result;

            // Act
            var responseResult = await apiClient.GetTickerOverviewResponseAsync(marketEnum, ticker);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<TickerOverviewResponse>(responseResult),
                () => Assert.NotNull(responseResult.Results),
                () => Assert.NotNull(responseResult.Results!.Address),
                () => Assert.NotNull(responseResult.Results!.Branding)
            );

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetTickerOverviewResponseAsync_ComplexResponse_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("AAPL", "2026-05-13", "2026-05-15")]
        public async Task GetShortVolumeResponseAsync_SingleParameter_Ticker_ReturnSuccessResponse(
            string ticker, string fromStr, string toStr)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            
            // Act
            var responseResult = await apiClient.GetShortVolumeResponseAsync(ticker, fromDate, toDate);
            
            // Assert
            Assert.Multiple(
                () => Assert.IsType<ShortVolumeResponse>(responseResult),
                () => Assert.NotEmpty(responseResult.Results),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Date > default(DateTime)))); // verifies complex serialization

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetShortVolumeResponseAsync_SingleParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }
    }
}
