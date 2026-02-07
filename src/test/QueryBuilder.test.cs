using ApiClient.Marketstack.Services;

namespace ApiClient.Marketstack.xUnitTests.Unit
{
    [Trait(nameof(TestAttributeNames.Category), "Unit")]
    public class QueryBuilder_Test
    {
        [Fact]
        public void AddParamter_StringType_NotDuplicate_IsSuccess()
        {
            // Arrange
            var queryBuilder = new QueryBuilder();
            var paramKey = "param";
            var paramValue = "value";

            // Act
            queryBuilder.AddParameter(paramKey, paramValue);
            
            // Assert
            Assert.True(queryBuilder.Parameters.Count == 1);
            Assert.True(queryBuilder.Parameters[paramKey] == paramValue);
        }

        [Fact]
        public void ToString_EmptyParamters_ReturnsQuestionmark()
        {
            // Arrange
            var queryBuilder = new QueryBuilder();
            
            // Act
            var queryString = queryBuilder.ToString();            
            
            // Assert
            Assert.True(queryString == "?");
        }
    }
}
