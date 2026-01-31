using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
 using Microsoft.Extensions.Configuration;
 
namespace ApiClient.Marketstack.xUnitTests
{
    public class ConfigurationFixture : IDisposable
    {
        public ConfigurationFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddUserSecrets<ConfigurationFixture>()
                .Build();
        }
        
        public IConfiguration Configuration { get; }

        public void Dispose() => GC.SuppressFinalize(this);
    }

    [Trait(nameof(TestAttributeNames.Category), "Unit")]
    public class MarketstackTestsUnit
    {
        [Fact]
        public void AssertTrue_ReturnsTrue()
        {
            Assert.True(true);
        }
    }

    [Trait(nameof(TestAttributeNames.Category), "Integration")]
    public class MarketstackTestsIntegration : IClassFixture<ConfigurationFixture>
    {
        ConfigurationFixture _fixture;

        public MarketstackTestsIntegration(ConfigurationFixture fixture)
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
            var symbol = "MSFT";
            var date = new DateTime(2026, 1, 5);

            // Act
            var result = await apiClient.GetEodDataAsync(symbol, date);

            // Assert
            Assert.IsType<EodResponse>(result);
            Assert.IsType<EodData[]>(result.Data);
            Assert.IsType<Pagination>(result.Pagination);

            Assert.True(result.Data.Length > 0);
            
            Debug.WriteLine(result.Data.First());
        }
    }
}
