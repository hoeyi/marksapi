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
            Assert.Single(queryBuilder.Parameters);
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
            Assert.Single(queryBuilder.Parameters);
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
            var initParams = new Dictionary<string, string>(){{ "param", "value" }};

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
            var initParams = new Dictionary<string, string>(){{ "param", "value" }};
            
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
            Assert.Equal("?", queryString);
        }

        [Fact]
        public void ConvertEndpoint_ToStringPattern()
        {
            // Arrange
            string pattern = "/api/{param1}/path/{param2}";
            string expected = "/api/{0}/path/{1}";

            // Act
            string observed = QueryBuilder.ConvertEndpointToStringPattern(pattern);
            
            // Assert
            Assert.Equal(expected, observed);
        }

        [Fact]
        public void ValidateDateRangeOrThrow_DateRangeInvalid_ThrowsArgumentException()
        {
            // Arrange
            var qb = new QueryBuilder();
            // Force dateTo to be less than dateFrom
            var dateFrom = DateTime.Now;
            var dateTo = dateFrom.AddDays(-1);
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => qb.ValidateDateRangeOrThrow(dateFrom, dateTo));
        }

        [Theory]
        [InlineData("2026-01-01", "2025-12-31")] // date range can have day-length < 0
        [InlineData("2026-01-01", "2026-02-01")] // date range can have day-length > 30
        public void ValidateDateRangeOrThrow_Static_DateRangeTooLong_ThrowsArgumentException(string dateFromStr, string dateToStr)
        {
            // Arrange
            var qb = new QueryBuilder();
            // Force dateTp to be more than 30 days after than dateFrom
            var dateFrom = DateTime.Parse(dateFromStr);
            var dateTo = DateTime.Parse(dateToStr);
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => qb.ValidateDateRangeOrThrow(dateFrom, dateTo));
        }

        [Fact]
        public void ValidateDateRangeOrThrow_Dynamic_DateRangeTooLong_ThrowsArgumentException_()
        {
            // Arrange
            var qb = new QueryBuilder();
            // Force dateTp to be more than 30 days after than dateFrom
            var dateFrom = DateTime.Now;
            var dateTo = dateFrom.AddDays(31);
            
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => qb.ValidateDateRangeOrThrow(dateFrom, dateTo));
        }

        [Theory]
        [InlineData("2026-01-01", "2026-01-01")] // date range can have day-length == 0
        [InlineData("2026-01-01", "2026-01-31")] // date range can have day-length <= 30
        public void ValidateDateRangeOrThrow_GoodRange_ReturnsTrue(string dateFromStr, string dateToStr)
        {
            // Arrange
            var qb = new QueryBuilder();
            var dateFrom = DateTime.Parse(dateFromStr);
            var dateTo = DateTime.Parse(dateToStr);

            // Act
            bool result = qb.ValidateDateRangeOrThrow(dateFrom, dateTo);
            
            // Assert
            Assert.True(result);
        }
    }
}
