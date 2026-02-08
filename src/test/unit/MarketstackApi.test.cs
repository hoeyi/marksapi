using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Marketstack.Services;
using Castle.Core.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace ApiClient.Marketstack.xUnitTests.Unit
{
    [Trait(nameof(TestAttributeNames.Category), "Unit")]
    public class MarketstackApi_Test(ConfigurationFixture fixture) : IClassFixture<ConfigurationFixture>
    {
        ConfigurationFixture _fixture = fixture;
        const string Test_ApiKey = "test-string";
        [Fact]
        public void Constructor_ReturnsNewInstance()
        {
            // Arrange
            // Act
            var apiClient = new MarketstackApi(apiKey: Test_ApiKey);            
            
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
            var apiClient = new MarketstackApi(apiKey: Test_ApiKey, logger: _fixture.Logger);
            
            // Assert
            Assert.IsType<MarketstackApi>(apiClient);
        }

        [Fact]
        public async Task GetEodResponse_ResponseIsValid_ReturnsEodResponse()
        {
            // Arrange
            var validResponse = new EodResponse();
            var mocks = new Mocks()
            {
                HttpMessageHandler = new Mock<HttpMessageHandler>()
            };
            mocks.HttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(validResponse))
                });
            var client = new HttpClient(mocks.HttpMessageHandler.Object);
            var service = new MarketstackApi(client, Test_ApiKey);
            var symbol = "MSFT";
            var date = new DateTime(2026, 1, 5);

            // Act
            var result = await service.GetEodResponseAsync([symbol], date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(validResponse, result);
        }

        [Fact]
        public async Task GetEodResponse_ResponseIsInvalid_ThrowsInvaliddOperationException()
        {
            // Arrange
            var mocks = new Mocks()
            {
                HttpMessageHandler = new Mock<HttpMessageHandler>()
            };
            mocks.HttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(string.Empty)
                });
            
            var client = new HttpClient(mocks.HttpMessageHandler.Object);
            var service = new MarketstackApi(client, Test_ApiKey);
            var symbol = "MSFT";
            var date = new DateTime(2026, 1, 5);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                service.GetEodResponseAsync([symbol], date));
        }

        [Fact]
        public async Task GetEodResponse_WhenEnsureSuccessStatusException_ThrowsHttpRequestException()
        {
            // Arrange
            var mocks = new Mocks()
            {
                HttpMessageHandler = new Mock<HttpMessageHandler>()
            };
            mocks.HttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            
            var client = new HttpClient(mocks.HttpMessageHandler.Object);
            var service = new MarketstackApi(client, Test_ApiKey);
            var symbol = "MSFT";
            var date = new DateTime(2026, 1, 5);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => 
                service.GetEodResponseAsync([symbol], date));
        }



        record Mocks
        {
            public Mock<MarketstackApi>? MarketstackApi { get; set; }
            public Mock<QueryBuilder>? QueryBuilder { get; set; }
            public Mock<HttpMessageHandler>? HttpMessageHandler { get; set; }
            public Mock<ILogger>? Logger { get; set; }
        }
    }
}
