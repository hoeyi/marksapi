using System.Collections.Generic;
using System.Linq;
using ApiClient.Services;

namespace ApiClient.Test.Unit
{
    [Trait(nameof(TestAttributeName.Category), "Unit")]
    public class QueryBuilder_Test
    {
        [Fact]
        public void AddParamter_DuplicateKeyNot_IsSuccess()
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
        public void AddParamter_DuplicateKey_IsSuccess()
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

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        public void AddParameter_EmptyKey_ThrowsException(string? paramKey)
        {
            // Arrange
            var queryBuilder = new QueryBuilder();
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(
                () => queryBuilder.AddParameter(paramKey!, "value")
            );
        }

        [Fact]
        public void AddParameter_NullKey_ThrowsException()
        {
            // Arrange
            var queryBuilder = new QueryBuilder();
            
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(
                () => queryBuilder.AddParameter(null!, "value")
            );
        }

        [Fact]
        public void RemoveParameter_ParameterExists_IsSuccess()
        {
            // Arrange
            var initParams = new Dictionary<string, string>(){{ "param", "value" }}
                                .ToArray();
            var queryBuilder = new QueryBuilder(initParams);
            if(queryBuilder.Parameters.Count != 1) 
                throw new InvalidOperationException("Improper test arrangement.");
            
            // Act
            queryBuilder.RemoveParameter("param");
            
            // Assert
            Assert.Empty(queryBuilder.Parameters); // collection is empty
            Assert.False(queryBuilder.Parameters.ContainsKey("param"));
        }

        [Fact]
        public void RemoveParameter_ParameterDoesNotExist_DoesNothing()
        {
            // Arrange
            var initParams = new Dictionary<string, string>(){{ "param", "value" }}
                                .ToArray();
            var queryBuilder = new QueryBuilder(initParams);
            if(queryBuilder.Parameters.Count != 1) 
                throw new InvalidOperationException("Improper test arrangement.");
            
            // Act
            queryBuilder.RemoveParameter("nonexistent-param");
            
            // Assert
            Assert.Single(queryBuilder.Parameters);
            Assert.True(queryBuilder.Parameters.ContainsKey("param"));
        }

        [Fact]
        public void ToString_OneParameter_ReturnsExpectedString()
        {
            // Arrange
            var queryBuilder = new QueryBuilder();
            var paramKey = "param";
            var paramValue = "value";

            // Act
            queryBuilder.AddParameter(paramKey, paramValue);
            var queryString = queryBuilder.ToString();

            // Assert
            Assert.Equal(expected: "?param=value", queryString);
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
