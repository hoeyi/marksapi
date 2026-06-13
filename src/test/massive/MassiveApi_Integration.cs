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
        [InlineData("AAPL", 1, "Day", "2025-11-25", "2025-11-28", 5)]
        public async Task GetStocksAggregateBarResponseAsync_ReturnSuccessResponse(
            string ticker, int multiplier, string timeSpan, string fromStr, string toStr, int limit)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            if(!Enum.TryParse(timeSpan, out BarTimespanEnum result))
                throw new ArgumentException(
                    $"Failed to parse test method arguments. Name: {nameof(timeSpan)}");

            // Act
            var responseResult = await apiClient.GetStocksAggregateBarResponseAsync(
                ticker, multiplier, result, fromDate, toDate, limit);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateBarResponse>(responseResult), 
                () => Assert.Equal(3, responseResult.ResultsCount),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Close > 0))); // verifies complex serialization

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetStocksAggregateBarResponseAsync_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("SPY251219C00650000", 1, "Day", "2025-11-25", "2025-11-28", 5)]
        public async Task GetOptionsAggregateBarResponseAsync_ReturnSuccessResponse(
            string ticker, int multiplier, string timeSpan, string fromStr, string toStr, int limit)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            if(!Enum.TryParse(timeSpan, out BarTimespanEnum result))
                throw new ArgumentException(
                    $"Failed to parse test method arguments. Name: {nameof(timeSpan)}");

            // Act
            var responseResult = await apiClient.GetOptionsAggregateBarResponseAsync(
                ticker, multiplier, result, fromDate, toDate, limit);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateBarResponse>(responseResult), 
                () => Assert.Equal(3, responseResult.ResultsCount),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Close > 0))); // verifies complex serialization

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetOptionsAggregateBarResponseAsync_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("COMP", 1, "Day", "2025-11-25", "2025-11-28", 5)]
        public async Task GetIndexAggregateBarResponseAsync_ReturnSuccessResponse(
            string ticker, int multiplier, string timeSpan, string fromStr, string toStr, int limit)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            if(!Enum.TryParse(timeSpan, out BarTimespanEnum result))
                throw new ArgumentException(
                    $"Failed to parse test method arguments. Name: {nameof(timeSpan)}");

            // Act
            var responseResult = await apiClient.GetIndexAggregateBarResponseAsync(
                ticker, multiplier, result, fromDate, toDate, limit);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateBarResponse>(responseResult), 
                () => Assert.Equal(3, responseResult.QueryCount),
                () => Assert.All(responseResult.Results, x => Assert.True(x.Close > 0))); // verifies complex serialization

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetIndexAggregateBarResponseAsync_ReturnSuccessResponse), 
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
