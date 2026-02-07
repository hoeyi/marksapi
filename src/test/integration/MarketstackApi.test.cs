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
            ArgumentException.ThrowIfNullOrWhiteSpace(_fixture.Configuration["api_base_uri"]);
        }

        [Theory]
        [InlineData(new[]{"MSFT"}, "2026-01-05")]
        [InlineData(new[]{"AAPL"}, "2026-01-05")]
        public async Task GetEodDataAsync_ReturnSuccessResponse(string[] symbols, string dateStr)
        {
            // Arrange
            var apiClient = new MarketstackApi(_fixture.Configuration["api_key"]!);
            var date = DateTime.Parse(dateStr);

            // Act
            var result = await apiClient.GetEodDataAsync(symbols, date);

            // Assert
            Assert.IsType<EodResponse>(result);
            Assert.IsType<EodData[]>(result.Data);
            Assert.IsType<Pagination>(result.Pagination);
            Assert.True(result.Data.Length > 0);

            // Print result            
            _fixture.Logger.LogInformation("{@pagination}", result.Pagination);
            _fixture.Logger.LogInformation("{@data}", result.Data);

            Debug.WriteLine(result.Data.First());
        }
    }
}
