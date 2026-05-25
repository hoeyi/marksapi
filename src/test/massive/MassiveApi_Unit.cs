using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Marketstack;
using ApiClient.Massive;
using ApiClient.Services;
using Castle.Core.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

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
            Assert.IsType<MarketstackApi>(apiClient);
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
    }
}
