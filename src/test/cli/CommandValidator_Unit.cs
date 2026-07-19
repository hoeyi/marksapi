using System;
using ApiClient.Massive;
using ApiClient.Services;
using Ichyd.Marksapi.Cli;
using Ichyd.Marksapi.Cli.Massive.Verbs;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ApiClient.Test.Marksapi.Unit
{
    [Trait(nameof(TestAttributeName.Category), "Unit")]
    public class CommandValidator_Test : IClassFixture<Fixture>
    {
        private Interval<int> _queryLimit;
        private Fixture _fixture;
        public CommandValidator_Test(Fixture fixture)
        {
            _fixture = fixture;
            QueryOptions options = new();
            var section = _fixture.Configuration
                .GetSection("massive")?
                .GetSection(nameof(QueryOptions));
            if (section is null)
            {
                options.UpperLimit = 5000;
                options.LowerLimit = 1;
            }
            else
                section.Bind(options);

            _queryLimit = new(options.LowerLimit, options.UpperLimit);
        }

        #region ValidateDateRangeOrThrow

        [Fact]
        public void ValidateDateRangeOrThrow_WhenBothDatesValid_ReturnsSameInstance()
        {
            // Arrange
            var validator = new CommandValidator();
            var fromDate = DateTime.Now.AddDays(-7);
            var toDate = DateTime.Now;

            // Act
            var result = validator.ValidateDateRangeOrThrow(fromDate, toDate);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateDateRangeOrThrow_FromDateNull_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var fromDate = (DateTime?)null;
            var toDate = DateTime.Now;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateDateRangeOrThrow(fromDate, toDate));
            Assert.Contains(nameof(fromDate), ex.Message);
            Assert.Contains(nameof(toDate), ex.Message);
        }

        [Fact]
        public void ValidateDateRangeOrThrow_ToDateNull_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var fromDate = DateTime.Now.AddDays(-7);
            var toDate = (DateTime?)null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateDateRangeOrThrow(fromDate, toDate));
            Assert.Contains(nameof(fromDate), ex.Message);
            Assert.Contains(nameof(toDate), ex.Message);
        }

        [Fact]
        public void ValidateDateRangeOrThrow_BothDatesNull_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var fromDate = (DateTime?)null;
            var toDate = (DateTime?)null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateDateRangeOrThrow(fromDate, toDate));
            Assert.Contains(nameof(fromDate), ex.Message);
            Assert.Contains(nameof(toDate), ex.Message);
        }

        #endregion

        #region ValidateFormatOrThrow

        [Theory]
        [InlineData("csv")]
        [InlineData("json")]
        [InlineData("console")]
        [InlineData("CSV")]
        [InlineData("JSON")]
        public void ValidateFormatOrThrow_ValidFormat_ReturnsSameInstance(string format)
        {
            // Arrange
            var validator = new CommandValidator();

            // Act
            var result = validator.ValidateFormatOrThrow(format);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateFormatOrThrow_NullFormat_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            string? format = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateFormatOrThrow(format));
            Assert.Contains(nameof(format), ex.Message);
        }

        [Fact]
        public void ValidateFormatOrThrow_EmptyString_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var format = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateFormatOrThrow(format));
            Assert.Contains(nameof(format), ex.Message);
        }

        [Fact]
        public void ValidateFormatOrThrow_InvalidFormat_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var format = "xml";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateFormatOrThrow(format));
            Assert.Contains(nameof(format), ex.Message);
        }

        #endregion

        #region ValidateLimitOrThrow

        [Theory]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(999)]
        public void ValidateLimitOrThrow_LimitWithinInterval_ReturnsSameInstance(int limit)
        {
            // Arrange
            var validator = new CommandValidator();
            var interval = _queryLimit;
            int? limitValue = limit;

            // Act
            var result = validator.ValidateLimitOrThrow(limitValue, interval);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateLimitOrThrow_LimitBelowInterval_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var interval = _queryLimit;
            int? limit = _queryLimit.Start - 1;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateLimitOrThrow(limit, interval));
            Assert.Contains(nameof(limit), ex.Message);
        }

        [Fact]
        public void ValidateLimitOrThrow_LimitAboveInterval_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var interval = _queryLimit;
            int? limit = _queryLimit.End + 1;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateLimitOrThrow(limit, interval));
            Assert.Contains(nameof(limit), ex.Message);
        }

        [Fact]
        public void ValidateLimitOrThrow_LimitIsNull_DoesNotThrowAndReturnsSameInstance()
        {
            // Arrange
            var validator = new CommandValidator();
            var interval = _queryLimit;
            int? limitValue = null;

            // Act
            var result = validator.ValidateLimitOrThrow(limitValue, interval);

            // Assert
            Assert.Same(validator, result);
        }

        #endregion

        #region ValidateMarketOrThrow

        [Theory]
        [InlineData("crypto", Market.Crypto)]
        [InlineData("fx", Market.Fx)]
        [InlineData("indices", Market.Indices)]
        [InlineData("options", Market.Options)]
        [InlineData("stocks", Market.Stocks)]
        [InlineData("CRYPTO", Market.Crypto)]
        public void ValidateMarketOrThrow_ValidMarket_ReturnsSameInstanceAndSetsOutputEnum(string market, Market expectedEnum)
        {
            // Arrange
            var validator = new CommandValidator();

            // Act
            var result = validator.ValidateMarketOrThrow(market, out var mktEnum);

            // Assert
            Assert.Same(validator, result);
            Assert.Equal(expectedEnum, mktEnum);
        }

        [Fact]
        public void ValidateMarketOrThrow_NullMarket_ThrowsArgumentNullException()
        {
            // Arrange
            var validator = new CommandValidator();
            string? market = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => 
                validator.ValidateMarketOrThrow(market, out _));
        }

        [Fact]
        public void ValidateMarketOrThrow_EmptyString_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var market = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateMarketOrThrow(market, out _));
            Assert.Contains(nameof(market), ex.Message);
        }

        [Fact]
        public void ValidateMarketOrThrow_WhitespaceOnly_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var market = "   ";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateMarketOrThrow(market, out _));
            Assert.Contains(nameof(market), ex.Message);
        }

        [Fact]
        public void ValidateMarketOrThrow_InvalidMarket_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var market = "invalid";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateMarketOrThrow(market, out _));
            Assert.Contains(nameof(market), ex.Message);
        }

        #endregion

        #region ValidateRatioRangeOrThrow

        [Theory]
        [InlineData(0.1f, 0.5f)]
        [InlineData(0.3f, 0.8f)]
        [InlineData(0.0f, 1.0f)]
        public void ValidateRatioRangeOrThrow_MinLessThanMax_ReturnsSameInstance(float min, float max)
        {
            // Arrange
            var validator = new CommandValidator();

            // Act
            var result = validator.ValidateRatioRangeOrThrow(min, max);

            // Assert
            Assert.Same(validator, result);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(0.5f, null)]
        [InlineData(null, 0.5f)]
        public void ValidateRatioRangeOrThrow_OneOrBothNull_ReturnsSameInstance(float? min, float? max)
        {
            // Arrange
            var validator = new CommandValidator();

            // Act
            var result = validator.ValidateRatioRangeOrThrow(min, max);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateRatioRangeOrThrow_MinGreaterThanMax_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var ratioMin = 0.8f;
            var ratioMax = 0.3f;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateRatioRangeOrThrow(ratioMin, ratioMax));
            Assert.Contains(nameof(ratioMin), ex.Message);
            Assert.Contains(nameof(ratioMax), ex.Message);
        }

        [Fact]
        public void ValidateRatioRangeOrThrow_MinEqualsMax_ReturnsSameInstance()
        {
            // Arrange
            var validator = new CommandValidator();
            var ratioMin = 0.5f;
            var ratioMax = 0.5f;

            // Act
            var result = validator.ValidateRatioRangeOrThrow(ratioMin, ratioMax);

            // Assert
            Assert.Same(validator, result);
        }

        #endregion

        #region ValidateTickerOrThrow

        [Theory]
        [InlineData("AAPL")]
        [InlineData("BTCUSD")]
        [InlineData("EURUSD")]
        public void ValidateTickerOrThrow_NonEmptyTicker_ReturnsSameInstance(string ticker)
        {
            // Arrange
            var validator = new CommandValidator();

            // Act
            var result = validator.ValidateTickerOrThrow(ticker);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateTickerOrThrow_NullTicker_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            string? ticker = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateTickerOrThrow(ticker));
            Assert.Contains(nameof(ticker), ex.Message);
        }

        [Fact]
        public void ValidateTickerOrThrow_EmptyString_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var ticker = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateTickerOrThrow(ticker));
            Assert.Contains(nameof(ticker), ex.Message);
        }

        #endregion

        #region ValidateTickerOrTickersOrThrow

        [Fact]
        public void ValidateTickerOrTickersOrThrow_TickerSpecified_ReturnsSameInstance()
        {
            // Arrange
            var validator = new CommandValidator();
            var ticker = "AAPL";
            string? tickers = null;

            // Act
            var result = validator.ValidateTickerOrTickersOrThrow(ticker, tickers);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateTickerOrTickersOrThrow_TickersSpecified_ReturnsSameInstance()
        {
            // Arrange
            var validator = new CommandValidator();
            string? ticker = null;
            var tickers = "AAPL,BTCUSD";

            // Act
            var result = validator.ValidateTickerOrTickersOrThrow(ticker, tickers);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateTickerOrTickersOrThrow_BothSpecified_ReturnsSameInstance()
        {
            // Arrange
            var validator = new CommandValidator();
            var ticker = "AAPL";
            var tickers = "BTCUSD,ETHUSD";

            // Act
            var result = validator.ValidateTickerOrTickersOrThrow(ticker, tickers);

            // Assert
            Assert.Same(validator, result);
        }

        [Fact]
        public void ValidateTickerOrTickersOrThrow_BothNull_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            string? ticker = null;
            string? tickers = null;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateTickerOrTickersOrThrow(ticker, tickers));
            Assert.Contains(nameof(ticker), ex.Message);
            Assert.Contains(nameof(tickers), ex.Message);
        }

        [Fact]
        public void ValidateTickerOrTickersOrThrow_BothEmptyStrings_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var ticker = "";
            var tickers = "";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateTickerOrTickersOrThrow(ticker, tickers));
            Assert.Contains(nameof(ticker), ex.Message);
            Assert.Contains(nameof(tickers), ex.Message);
        }

        [Fact]
        public void ValidateTickerOrTickersOrThrow_BothWhitespaces_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var ticker = "   ";
            var tickers = "   ";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateTickerOrTickersOrThrow(ticker, tickers));
            Assert.Contains(nameof(ticker), ex.Message);
            Assert.Contains(nameof(tickers), ex.Message);
        }

        #endregion

        #region ValidateTimespanOrThrow

        [Theory]
        [InlineData("second", BarTimespan.Second)]
        [InlineData("minute", BarTimespan.Minute)]
        [InlineData("hour", BarTimespan.Hour)]
        [InlineData("day", BarTimespan.Day)]
        [InlineData("week", BarTimespan.Week)]
        [InlineData("month", BarTimespan.Month)]
        [InlineData("quarter", BarTimespan.Quarter)]
        [InlineData("year", BarTimespan.Year)]
        [InlineData("SECOND", BarTimespan.Second)]
        [InlineData("Minute", BarTimespan.Minute)]
        public void ValidateTimespanOrThrow_ValidTimespan_ReturnsSameInstanceAndSetsOutputEnum(string timespan, BarTimespan expectedEnum)
        {
            // Arrange
            var validator = new CommandValidator();

            // Act
            var result = validator.ValidateTimespanOrThrow(timespan, out var barTimespan);

            // Assert
            Assert.Same(validator, result);
            Assert.Equal(expectedEnum, barTimespan);
        }

        [Fact]
        public void ValidateTimespanOrThrow_NullTimespan_ReturnsSameInstanceWithNullOutput()
        {
            // Arrange
            var validator = new CommandValidator();
            string? timespan = null;

            // Act
            var result = validator.ValidateTimespanOrThrow(timespan, out var barTimespan);

            // Assert
            Assert.Same(validator, result);
            Assert.Null(barTimespan);
        }

        [Fact]
        public void ValidateTimespanOrThrow_EmptyString_ReturnsSameInstanceWithNullOutput()
        {
            // Arrange
            var validator = new CommandValidator();
            var timespan = "";

            // Act
            var result = validator.ValidateTimespanOrThrow(timespan, out var barTimespan);

            // Assert
            Assert.Same(validator, result);
            Assert.Null(barTimespan);
        }

        [Fact]
        public void ValidateTimespanOrThrow_InvalidTimespan_ThrowsArgumentException()
        {
            // Arrange
            var validator = new CommandValidator();
            var timespan = "invalid";

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => 
                validator.ValidateTimespanOrThrow(timespan, out _));
            Assert.Contains(nameof(timespan), ex.Message);
        }
        #endregion

        #region  ValidateFileOutputOrThrow
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("./runtimes")]
        public void ValidateFileOuputOrThrow_Validstring_ReturnsCommandInstance(
            string? path)
        {
            // Arrange
            var validator = new CommandValidator();

            // Act
            var validatorObs = validator.ValidateFileOuputOrThrow(path);
            
            // Assert
            // Equality by reference should return true.
            Assert.Equal(validator, validatorObs);
        }

        [Fact]
        public void ValidateFileOuputOrThrow_MissingDirectory_DirectoryNotFoundException()
        {
            // Arrange
            var validator = new CommandValidator();
            var pathDoesNotExist = "/NOT/A/REAL/PATH";
            
            // Act & Assert
            var ex = Assert.Throws<DirectoryNotFoundException>(() => 
                validator.ValidateFileOuputOrThrow(pathDoesNotExist));
            Assert.Contains(pathDoesNotExist, ex.Message);
        }
        #endregion
    }
}