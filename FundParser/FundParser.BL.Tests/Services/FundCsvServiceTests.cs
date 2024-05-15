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
    private const string TestCsvString = "testCsvString";
    private const string Url = "http://supercoolwebsite.com/fund.csv";

    private Mock<IHoldingService> holdingServiceMock;
    private Mock<ICsvParsingService<FundCsvRow>> csvParsingServiceMock;
    private Mock<IDownloaderService> downloaderServiceMock;
    private Mock<IConfiguration> configurationMock;
    private Mock<IConfigurationSection> configurationSectionMock;
    private Mock<IUnitOfWork> unitOfWorkMock;
    private FundCsvService fundCsvService;

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

    [Test]
    public async Task UpdateHoldings_ParseFailed_ThrowsException()
    {
        // Arrange
        downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestCsvString);

        csvParsingServiceMock.Setup(m => m.ParseString(TestCsvString, It.IsAny<CancellationToken>()))
            .Returns([]);

        // Act
        var result = await fundCsvService.UpdateHoldings();

        // Assert
        Assert.That(result, Is.EqualTo(-1));
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
        downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestCsvString);

        var shares = 1000;
        csvParsingServiceMock.Setup(m => m.ParseString(TestCsvString, It.IsAny<CancellationToken>()))
            .Returns(new List<FundCsvRow>
            {
                    new FundCsvRow
                    {
                        Fund = "ARKK",
                        Cusip = "12345",
                        Ticker = "ABC",
                        Company = "Test Company",
                        Shares = shares.ToString(),
                        MarketValue = "$10000",
                        Weight = "10%",
                        Date = "01/01/2022"
                    }
            });

        // Act
        var result = await fundCsvService.UpdateHoldings();

        // Assert
        holdingServiceMock.Verify(
            holdingService => holdingService.AddHolding(It.Is<AddHoldingDTO>(holding => holding.Shares == shares), It.IsAny<CancellationToken>()),
            Times.Once());
        unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once());
        Assert.That(result, Is.EqualTo(1));
    }
}