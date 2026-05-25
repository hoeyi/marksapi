using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ApiClient.Marketstack;
using ApiClient.Massive;
using ApiClient.Massive.Response;
using ApiClient.Services;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace ApiClient.Test.Massive.Integration
{
    [Trait(nameof(TestAttributeName.Category), "Integration")]
    public class MassiveApi_Test : IClassFixture<ConfigurationFixture>
    {
        ConfigurationFixture _fixture;
        public MassiveApi_Test(ConfigurationFixture fixture)
        {
            _fixture = fixture;
            ArgumentException.ThrowIfNullOrWhiteSpace(_fixture.Configuration["api_key:massive"]);
        }

        [Theory]
        [InlineData("AAPL", 1, "Day", "2025-11-25", "2025-11-28", 5)]
        public async Task GetEodDataAsync_ReturnSuccessResponse(
            string ticker, int multiplier, string timeSpan, string fromStr, string toStr, int limit)
        {
            // Arrange
            var apiClient = new MassiveApi(_fixture.Configuration["api_key:massive"]!);
            var fromDate = DateTime.Parse(fromStr);
            var toDate = DateTime.Parse(toStr);
            if(!Enum.TryParse(timeSpan, out BarTimespanEnum result))
                throw new ArgumentException(
                    $"Failed to parse test method arguments. Name: {nameof(timeSpan)}");

            // Act
            var responsResult = await apiClient.GetAggregateBarResponseAsync(ticker, multiplier, result, fromDate, toDate, limit);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<AggregateBarResponse>(responsResult), 
                () => Assert.Equal(3, responsResult.ResultsCount));

            // Print result            
            _fixture.Logger.LogDebug("{@responsResult}", responsResult);
        }
    }
}
