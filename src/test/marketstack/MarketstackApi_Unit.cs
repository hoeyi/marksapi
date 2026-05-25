using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Marketstack;
using ApiClient.Services;
using Castle.Core.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace ApiClient.Test.Marketstack.Unit
{
    [Trait(nameof(TestAttributeName.Category), "Unit")]
    public partial class MarketstackApi_Test
    {
        const string Test_ApiKey = "test-string";
        record Mocks
        {
            public Mock<MarketstackApi>? MarketstackApi { get; set; }
            public Mock<QueryBuilder>? QueryBuilder { get; set; }
            public Mock<HttpMessageHandler>? HttpMessageHandler { get; set; }
            public Mock<ILogger>? Logger { get; set; }
        }
    }
    #region Constructor / Common
    public partial class MarketstackApi_Test
    {
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
        [InlineData("\t")]
        [InlineData("\r")]
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
        public void ValidateDateRangeOrThrow_DateRangeInvalid_ThrowsArgumentException()
        {
            // Arrange
            var apiClient = new MarketstackApi(apiKey: Test_ApiKey);
            // Force dateTo to be less than dateFrom
            var dateFrom = DateTime.Now;
            var dateTo = dateFrom.AddDays(-1);
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => apiClient.ValidateDateRangeOrThrow(dateFrom, dateTo));
        }

        [Theory]
        [InlineData("2026-01-01", "2025-12-31")] // date range can have day-length < 0
        [InlineData("2026-01-01", "2026-02-01")] // date range can have day-length > 30
        public void ValidateDateRangeOrThrow_Static_DateRangeTooLong_ThrowsArgumentException(string dateFromStr, string dateToStr)
        {
            // Arrange
            var apiClient = new MarketstackApi(apiKey: Test_ApiKey);
            // Force dateTp to be more than 30 days after than dateFrom
            var dateFrom = DateTime.Parse(dateFromStr);
            var dateTo = DateTime.Parse(dateToStr);
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => apiClient.ValidateDateRangeOrThrow(dateFrom, dateTo));
        }

        [Fact]
        public void ValidateDateRangeOrThrow_Dynamic_DateRangeTooLong_ThrowsArgumentException_()
        {
            // Arrange
            var apiClient = new MarketstackApi(apiKey: Test_ApiKey);
            // Force dateTp to be more than 30 days after than dateFrom
            var dateFrom = DateTime.Now;
            var dateTo = dateFrom.AddDays(31);
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => apiClient.ValidateDateRangeOrThrow(dateFrom, dateTo));
        }

        [Theory]
        [InlineData("2026-01-01", "2026-01-01")] // date range can have day-length == 0
        [InlineData("2026-01-01", "2026-01-31")] // date range can have day-length <= 30
        public void ValidateDateRangeOrThrow_GoodRange_ReturnsTrue(string dateFromStr, string dateToStr)
        {
            // Arrange
            var apiClient = new MarketstackApi(apiKey: Test_ApiKey);
            var dateFrom = DateTime.Parse(dateFromStr);
            var dateTo = DateTime.Parse(dateToStr);

            // Act
            bool result = apiClient.ValidateDateRangeOrThrow(dateFrom, dateTo);
            
            // Assert
            Assert.True(result);
        }
    }
    #endregion

    #region Endpoint: { /eod }
    public partial class MarketstackApi_Test
    {
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
    }
    #endregion Endpoint: { /eod }
}
