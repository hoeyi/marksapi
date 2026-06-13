using ApiClient.Massive;
using ApiClient.Services;
using Castle.Core.Logging;
using Moq;

namespace ApiClient.Test.Massive.Unit
{
    [Trait(nameof(TestAttributeName.Category), "Unit")]
    public partial class MassiveApi_Test
    {
        const string Test_ApiKey = "test-string";
        record Mocks
        {
            public Mock<MassiveApi>? MarketstackApi { get; set; }
            public Mock<QueryBuilder>? QueryBuilder { get; set; }
            public Mock<HttpMessageHandler>? HttpMessageHandler { get; set; }
            public Mock<ILogger>? Logger { get; set; }
        }
    }
    public partial class MassiveApi_Test
    {
        [Fact]
        public void Constructor_ReturnsNewInstance()
        {
            // Arrange
            // Act
            var apiClient = new MassiveApi(apiKey: Test_ApiKey);

            // Assert
            Assert.IsType<MassiveApi>(apiClient);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        public void Constructor_EmptyApiKey_ThrowsArgumentException(string apiKey)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new MassiveApi(apiKey));
        }

        [Fact]
        public void Constructor_NullApiKey_ThrowsArgumentException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MassiveApi(null!));
        }

        [Theory]
        [InlineData("Stocks", "AAPL,MSFT")]
        public async Task GetAllTickerOverviewResponseAsync_ThrowNotImplementedException(
            string market, string tickersDelim)
        {
            // Arrange
            var apiClient = new MassiveApi(Test_ApiKey);
            string[] tickers = tickersDelim.Split(",");
            Market marketEnum;
            if(!Enum.TryParse(market, out Market result))
                throw new InvalidOperationException($"Test parameter '{market}' could not be parsed.");
            else
                marketEnum = result;

            // Act
            // Assert
            await Assert.ThrowsAsync<NotImplementedException>(
                () => apiClient.GetAllTickerOverviewResponseAsync(marketEnum, tickers));           
        }

        [Fact]
        public async Task GetStocksAggregateBarResponseAsync_ThrowNotImplementedException()
        {
            // Arrange
            var apiClient = new MassiveApi(Test_ApiKey);
            var market = Market.Stocks;
            string[] tickers = ["AAPL"];
            var multiplier = 1;
            var timeSpan = BarTimespanEnum.Day;
            var from = new DateTime(2025, 11, 25);
            var to = new DateTime(2025, 11, 28);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotImplementedException>(
                () => apiClient.GetAggregateBarResponseAsync(market, tickers, multiplier, timeSpan, from, to));           
        }
    }
}
