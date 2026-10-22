using ApiClient.Massive;
using ApiClient.Services;
using Castle.Core.Logging;
using Moq;

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
        public void Constructor_NullApiKey_ThrowsArgumentException()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MassiveApi(configuration: null!));
        }
    }
}
