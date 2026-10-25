using ApiClient.Cache.Entity;
using System;

namespace ApiClient.Test.Cache
{
    public class StructuredRequest_Unit
    {
        [Fact]
        public void AddParameter_NullValue_Throws_ArgumentNullException()
        {
            // Arrange
            var request = new StructuredRequest(Guid.NewGuid(), "a", DateTime.UtcNow);

            //Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => request.AddParameter(null!));        
        }

        [Fact]
        public void AddParameter_NewParameter_UpdatesParametersCount()
        {
            // Arrange
            var request = new StructuredRequest(Guid.NewGuid(), "a", DateTime.UtcNow);
            int paramCount = request.Parameters.Count;

            //Act
            request.AddParameter(
                    new RequestParameter("a", "true", ParameterValueType.Boolean));

            // Assert
            Assert.NotEqual(paramCount, request.Parameters.Count);
            Assert.Single(request.Parameters);        
        }

        [Fact]
        public void AddParameter_ExistingParameter_Throws_InvalidOperationException()
        {
            // Arrange
            var request = new StructuredRequest(Guid.NewGuid(), "a", DateTime.UtcNow);
            request.AddParameter(new("a", "true", ParameterValueType.Boolean));

            //Act
            // Assert
            Assert.Throws<InvalidOperationException>(
                () => request.AddParameter(new("a", "false", ParameterValueType.Boolean)));            
        }
    }
}

