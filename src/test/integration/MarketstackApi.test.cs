using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ApiClient.Marketstack.xUnitTests.Integration
{
    [Trait(nameof(TestAttributeNames.Category), "Integration")]
    public class MarketstackApi_Test: IClassFixture<ConfigurationFixture>
    {
        ConfigurationFixture _fixture;
        public MarketstackApi_Test(ConfigurationFixture fixture)
        {
            _fixture = fixture;
            ArgumentException.ThrowIfNullOrWhiteSpace(_fixture.Configuration["api_key"]);
        }

        [Theory]
        [InlineData(new[]{"AAPL", "MSFT"}, "2026-01-05")]
        public async Task GetEodDataAsync_ReturnSuccessResponse(string[] symbols, string dateStr)
        {
            // Arrange
            var apiClient = new MarketstackApi(_fixture.Configuration["api_key"]!);
            var date = DateTime.Parse(dateStr);

            // Act
            var result = await apiClient.GetEodResponseAsync(symbols, date);

            // Assert
            Assert.IsType<EodResponse>(result);
            Assert.IsType<EodBar[]>(result.Data);
            Assert.IsType<Pagination>(result.Pagination);
            Assert.True(result.Data.Length > 0);

            // Print result            
            _fixture.Logger.LogInformation("{@pagination}", result.Pagination);
            _fixture.Logger.LogInformation("{@data}", result.Data);

            Debug.WriteLine(result.Data.First());
        }

        [Theory]
        [InlineData(new[]{"AAPL", "MSFT"}, "2026-01-05")]
        public async Task GetIntradayDataAsync_ReturnSuccessResponse(string[] symbols, string dateStr)
        {
            throw new NotImplementedException("Intraday endpoint is a paid tier.");

            // Arrange
#pragma warning disable CS0162 // Unreachable code detected
            var apiClient = new MarketstackApi(_fixture.Configuration["api_key"]!);
            var date = DateTime.Parse(dateStr);

            // Act
            var result = await apiClient.GetIntradayResponseAsync(symbols, date);

            // Assert
            Assert.IsType<IntradayResponse>(result);
            Assert.IsType<IntradayBar[]>(result.Data);
            Assert.IsType<Pagination>(result.Pagination);
            Assert.True(result.Data.Length > 0);

            // Print result            
            _fixture.Logger.LogInformation("{@pagination}", result.Pagination);
            _fixture.Logger.LogInformation("{@data}", result.Data);

            Debug.WriteLine(result.Data.First());
#pragma warning restore CS0162 // Unreachable code detected
        }
    }
}
