using ApiClient.Massive;
using ApiClient.Massive.Response;
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
                () => Assert.Equal(3, responseResult.ResultsCount));

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
                () => Assert.Equal(3, responseResult.ResultsCount));

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetOptionsAggregateBarResponseAsync_ReturnSuccessResponse), 
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
            var responseResult = await apiClient.GetStocksAllTickersAsync(ticker);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateTickerResponse>(responseResult), 
                () => Assert.Equal(1, responseResult.Count));

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetStocksAllTickerAsync_SingleParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }

        [Theory]
        [InlineData("AAPL")]
        public async Task GetTickerOverviewResponseAsync_SingleParameter_Ticker_ReturnSuccessResponse(
            string ticker)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);
            
            // Act
            var responseResult = await apiClient.GetStocksTickerOverviewResponseAsync(ticker);
            
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
                nameof(GetTickerOverviewResponseAsync_SingleParameter_Ticker_ReturnSuccessResponse), 
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
                () => Assert.NotEmpty(responseResult.Results)
            );

            // Print result            
            _fixture.Logger.LogInformation(
                "'{method}' returned:\n{@responseResult}", 
                nameof(GetShortVolumeResponseAsync_SingleParameter_Ticker_ReturnSuccessResponse), 
                responseResult);
        }
    }
}
