using System.Runtime.InteropServices;

namespace ApiClient.Marketstack.xUnitTests.Unit
{
    [Trait(nameof(TestAttributeNames.Category), "Unit")]
    public class MarketstackApi_Test(ConfigurationFixture fixture) : IClassFixture<ConfigurationFixture>
    {
        ConfigurationFixture _fixture = fixture;

        [Fact]
        public void Constructor_ReturnsNewInstance()
        {
            // Arrange
            // Act
            var apiClient = new MarketstackApi(apiKey: "test-string");            
            
            // Assert
            Assert.IsType<MarketstackApi>(apiClient);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Constructor_EmptyApiKey_ThrowsArgumentException(string apiKey)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new MarketstackApi(apiKey));
        }

        [Fact]
        public void Constructor_NullApiKey_ThrowsArgumentException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MarketstackApi(null!));
        }

        [Fact]
        public void Constructor_WithLogger_ReturnsInstance()
        {
            // Arrange
            // Act
            var apiClient = new MarketstackApi(apiKey: "test-string", logger: _fixture.Logger);
            
            // Assert
            Assert.IsType<MarketstackApi>(apiClient);
        }
    }
}
