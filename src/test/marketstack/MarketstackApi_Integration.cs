using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ApiClient.Marketstack;

namespace ApiClient.Test.Marketstack.Integration
{
    [Trait(nameof(TestAttributeName.Category), "Integration")]
    public class MarketstackApi_Test: IClassFixture<ConfigurationFixture>
    {
        ConfigurationFixture _fixture;
        public MarketstackApi_Test(ConfigurationFixture fixture)
        {
            _fixture = fixture;
            ArgumentException.ThrowIfNullOrWhiteSpace(_fixture.Configuration["api_key:marketstack"]);
        }

        [Theory]
        [InlineData(new[]{"AAPL", "MSFT"}, "2026-01-05")]
        public async Task GetEodDataAsync_ReturnSuccessResponse(string[] symbols, string dateStr)
        {
            // Arrange
            var apiClient = new MarketstackApi(_fixture.Configuration["api_key:marketstack"]!);
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
    }
}
