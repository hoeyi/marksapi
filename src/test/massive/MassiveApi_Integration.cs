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
        public async Task GetEodDataAsync_ReturnSuccessResponse(
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
            var responsResult = await apiClient.GetAggregateBarResponseAsync(ticker, multiplier, result, fromDate, toDate, limit);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateBarResponse>(responsResult), 
                () => Assert.Equal(3, responsResult.ResultsCount));

            // Print result            
            _fixture.Logger.LogDebug("{@responsResult}", responsResult);
        }

        [Theory]
        [InlineData("AAPL")]
        public async Task GetStocksTickerAsync_SingleParameter_Ticker_ReturnSuccessResponse(
            string ticker)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);

            // Act
            var responsResult = await apiClient.GetStocksTickerAsync(ticker);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<TickerAggregateResponse>(responsResult), 
                () => Assert.Equal(1, responsResult.Count));

            // Print result            
            _fixture.Logger.LogDebug("{@responsResult}", responsResult);
        }
    }
}
