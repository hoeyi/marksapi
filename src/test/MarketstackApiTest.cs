using System;
using System.Diagnostics;
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
        }

        [Fact]
        public void Configuration_ApiKey_IsDefined()
        {
            // Arrange
            
            // Act
            var api_key = _fixture.Configuration["api_key"];
            Debug.WriteLine($"Found api_key={api_key}");
            
            // Assert
            Assert.False(string.IsNullOrEmpty(api_key));
        }

        [Fact]
        public async Task GetEodDataAsync_ReturnSuccessResponse()
        {
            Assert.Fail();
        }
    }
}
