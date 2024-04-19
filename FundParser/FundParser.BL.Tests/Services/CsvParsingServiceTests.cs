using System.Globalization;

using FundParser.BL.Services.CsvParsingService;
using FundParser.BL.Services.LoggingService;
using FundParser.BL.Tests.Mocks.CsvFormats;

using Moq;

namespace FundParser.BL.Tests.Services
{
    [TestFixture]
    public class CsvParsingServiceTests
    {
        private CultureInfo _czCulture;
        private Mock<ILoggingService> _loggerMock;
        private CsvParsingService<BasicCsvFormat> _parser;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILoggingService>();
            _parser = new CsvParsingService<BasicCsvFormat>(_loggerMock.Object);
            _czCulture = new CultureInfo("cs-CZ");
        }

        [Test]
        public void ParseString_InvalidHeader_ReturnsNull()
        {
            // Setup
            var basicCsv = GetCsvString(new List<string>
            {
                "DateTime,String,Int",
                "04/04/2024,ARKK,1.23",
                "04/18/2023,ARKK2,1.21,10000",
                "04/18/2024,ARKK3,1.25,420,Extra"
            });

            //Act
            var result = _parser.ParseString(basicCsv)?.ToList();

            //Assert
            Assert.That(result, Is.Null);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void ParseString_VariousColumnsInRows_ReturnsOnlyValidRows()
        {
            // Setup
            var basicCsv = GetCsvString(new List<string>
                {
                    "DateTime,String,Decimal,Int",
                    "04/04/2024,ARKK,1.23",
                    "04/18/2023,ARKK2,1.21,10000",
                    "04/18/2024,ARKK3,1.25,420,Extra"
                }); ;

            // Act
            var result = _parser.ParseString(basicCsv)?.ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].DateTime, Is.EqualTo(DateTime.Parse("18/04/2023", _czCulture)));
                Assert.That(result[0].String, Is.EqualTo("ARKK2"));
                Assert.That(result[0].Decimal, Is.EqualTo(1.21));
                Assert.That(result[0].Int, Is.EqualTo(10000));
            });
        }

        [Test]
        public void ParseString_BasicCsv_ReturnsCorrectResult()
        {
            // Setup
            var basicCsv = GetCsvString(new List<string>
                {
                    "DateTime,String,Decimal,Int",
                    "04/04/2024,ARKK,1.23,69",
                    "04/18/2024,ARKK2,1.25,420"
                });

            // Act
            var result = _parser.ParseString(basicCsv)?.ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].DateTime, Is.EqualTo(DateTime.Parse("04/04/2024", _czCulture)));
                Assert.That(result[0].String, Is.EqualTo("ARKK"));
                Assert.That(result[0].Decimal, Is.EqualTo(1.23));
                Assert.That(result[0].Int, Is.EqualTo(69));
                Assert.That(result[1].DateTime, Is.EqualTo(DateTime.Parse("18/04/2024", _czCulture)));
                Assert.That(result[1].String, Is.EqualTo("ARKK2"));
                Assert.That(result[1].Decimal, Is.EqualTo(1.25));
                Assert.That(result[1].Int, Is.EqualTo(420));
            });
        }

        private static string GetCsvString(IEnumerable<string> rows)
        {
            return string.Join(Environment.NewLine, rows);
        }
    }
}
