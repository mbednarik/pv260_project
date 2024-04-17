using FundParser.BL.Services.CsvParserService;
using FundParser.BL.Services.LoggingService;
using FundParser.BL.Tests.Helpers.CsvFormats;
using Moq;
using System.Globalization;

namespace FundParser.BL.Tests
{
    public class CsvParserTests
    {
        private CsvParserService<BasicCsvFormat> parser;
        CultureInfo czCulture;
        private Mock<ILoggingService> logger;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILoggingService>();
            parser = new CsvParserService<BasicCsvFormat>(logger.Object);
            czCulture = new CultureInfo("cs-CZ");
        }

        [Test]
        public void CsvParser_ParseString_BasicCsvCorrect()
        {
            var basicCsv = "DateTime,String,Decimal,Int\n" +
                      "04/04/2024,ARKK,1.23,69\n" +
                      "04/18/2024,ARKK2,1.25,420\n";
            var result = parser.ParseString(basicCsv)?.ToList();
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].DateTime, Is.EqualTo(DateTime.Parse("04/04/2024", czCulture)));
                Assert.That(result[0].String, Is.EqualTo("ARKK"));
                Assert.That(result[0].Decimal, Is.EqualTo(1.23));
                Assert.That(result[0].Int, Is.EqualTo(69));
                Assert.That(result[1].DateTime, Is.EqualTo(DateTime.Parse("18/04/2024", czCulture)));
                Assert.That(result[1].String, Is.EqualTo("ARKK2"));
                Assert.That(result[1].Decimal, Is.EqualTo(1.25));
                Assert.That(result[1].Int, Is.EqualTo(420));
            });
        }

        [Test]
        public void CsvParser_ParseString_BasicCsvVariousColumnsInOneRow()
        {
            var basicCsv = "DateTime,String,Decimal,Int\n" +
                      "04/04/2024,ARKK,1.23\n" +
                      "04/18/2023,ARKK2,1.21,10000\n" +
                      "04/18/2024,ARKK3,1.25,420,Extra\n";
            var result = parser.ParseString(basicCsv)?.ToList();
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].DateTime, Is.EqualTo(DateTime.Parse("18/04/2023", czCulture)));
                Assert.That(result[0].String, Is.EqualTo("ARKK2"));
                Assert.That(result[0].Decimal, Is.EqualTo(1.21));
                Assert.That(result[0].Int, Is.EqualTo(10000));
            });
        }

        [Test]
        public void CsvParser_ParseString_BasicCsvInvalidHeader()
        {
            var basicCsv = "DateTime,String,Int\n" +
                      "04/04/2024,ARKK,1.23\n" +
                      "04/18/2023,ARKK2,1.21,10000\n" +
                      "04/18/2024,ARKK3,1.25,420,Extra\n";
            var result = parser.ParseString(basicCsv)?.ToList();
            Assert.That(result, Is.EqualTo(null));
        }
    }
}
