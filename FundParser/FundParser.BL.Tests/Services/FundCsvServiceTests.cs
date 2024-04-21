using FundParser.BL.DTOs;
using FundParser.BL.Services.CsvParsingService;
using FundParser.BL.Services.DownloaderService;
using FundParser.BL.Services.FundCsvService;
using FundParser.BL.Services.HoldingService;
using FundParser.BL.Services.LoggingService;
using FundParser.DAL.Models;
using FundParser.DAL.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FundParser.BL.Tests.Services;

[TestFixture]
public class FundCsvServiceTests
{
    public class UpdateHoldingsTests : FundCsvServiceTestsBase
    {
        [Test]
        public void UpdateHoldings_DownloadFailed_ThrowsException()
        {
            // Arrange
            downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                    It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null!);

            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(() => fundCsvService.UpdateHoldings(),
                    Throws.Exception.With.Message.EqualTo("Failed to download csv"));
                Assert.That(() => fundCsvService.UpdateHoldings(), Throws.Exception);
            });
        }

        [Test]
        public void UpdateHoldings_ParseFailed_ThrowsException()
        {
            // Arrange
            const string csvString = "Fund,Cusip,Ticker,Company,Shares,MarketValue,Weight,Date\n" +
                                     "ARKK,12345,ABC,Test Company,1000,$10000,10%,01/01/2022";
            downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                    It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(csvString);
            csvParsingServiceMock.Setup(m => m.ParseString(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns((List<FundCsvRow>)null!);


            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(() => fundCsvService.UpdateHoldings(),
                    Throws.Exception.With.Message.EqualTo("Failed to parse csv"));
                Assert.That(() => fundCsvService.UpdateHoldings(), Throws.Exception);
            });
        }

        [Test]
        public void UpdateHoldings_CsvUrlNotConfigured_ThrowsException()
        {
            // Arrange
            configurationSectionMock.SetupGet(m => m.Value).Returns((string)null!);

            // Act & Assert
            Assert.That(() => fundCsvService.UpdateHoldings(), Throws.Exception);
        }

        [Test]
        public async Task UpdateHoldings_SuccessfulUpdate_ReturnsCorrectCount()
        {
            // Arrange
            const string testCsvString = "testCsvString";
            downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                    It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(testCsvString);
            csvParsingServiceMock.Setup(m => m.ParseString(testCsvString, It.IsAny<CancellationToken>()))
                .Returns(new List<FundCsvRow>
                {
                    new FundCsvRow
                    {
                        Fund = "ARKK",
                        Cusip = "12345",
                        Ticker = "ABC",
                        Company = "Test Company",
                        Shares = "1000",
                        MarketValue = "$10000",
                        Weight = "10%",
                        Date = "01/01/2022"
                    }
                });

            // Act
            var result = await fundCsvService.UpdateHoldings();

            // Assert
            holdingServiceMock.Verify(
                holdingService => holdingService.AddHolding(It.IsAny<AddHoldingDTO>(), It.IsAny<CancellationToken>()),
                Times.Once());
            unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
            Assert.That(result, Is.EqualTo(1));
        }
    }

    public class FundCsvServiceTestsBase
    {
        private const string Url = "http://supercoolwebsite.com/fund.csv";

        protected Mock<IHoldingService> holdingServiceMock;
        protected Mock<ICsvParsingService<FundCsvRow>> csvParsingServiceMock;
        protected Mock<IDownloaderService> downloaderServiceMock;
        protected Mock<IConfiguration> configurationMock;
        protected Mock<IConfigurationSection> configurationSectionMock;
        protected Mock<IUnitOfWork> unitOfWorkMock;
        protected FundCsvService fundCsvService;

        [SetUp]
        public void SetUp()
        {
            // Mock IConfigurationSection
            configurationSectionMock = new Mock<IConfigurationSection>();
            configurationSectionMock.SetupGet(m => m.Value).Returns(Url);

            // Mock IConfiguration
            configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m.GetSection(It.IsAny<string>())).Returns(configurationSectionMock.Object);

            // Mock other dependencies
            holdingServiceMock = new Mock<IHoldingService>();
            csvParsingServiceMock = new Mock<ICsvParsingService<FundCsvRow>>();
            downloaderServiceMock = new Mock<IDownloaderService>();
            unitOfWorkMock = new Mock<IUnitOfWork>();

            // Initialize FundCsvService with mocked dependencies
            fundCsvService = new FundCsvService(
                holdingServiceMock.Object,
                csvParsingServiceMock.Object,
                downloaderServiceMock.Object,
                unitOfWorkMock.Object,
                new Mock<ILoggingService>().Object,
                configurationMock.Object
            );
        }
    }
}