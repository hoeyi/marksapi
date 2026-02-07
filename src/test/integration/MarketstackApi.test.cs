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

        [Fact]
        public async Task GetEodDataAsync_ReturnSuccessResponse()
        {
            // Arrange
            var apiClient = new MarketstackApi(_fixture.Configuration["api_key"]!);
            string[] symbol = ["MSFT"];
            var date = new DateTime(2026, 1, 5);

            // Act
            var result = await apiClient.GetEodDataAsync(symbol, date);

            // Assert
            Assert.IsType<EodResponse>(result);
            Assert.IsType<EodData[]>(result.Data);
            Assert.IsType<Pagination>(result.Pagination);
            Assert.True(result.Data.Length > 0);

            // Print result            
            _fixture.Logger.LogInformation("{result}", result);
            Debug.WriteLine(result.Data.First());
        }
    }
}
