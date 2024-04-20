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

public class FundCsvServiceTests
{
    private const string Url = "http://supercoolwebsite.com/fund.csv";

    private Mock<IHoldingService> _holdingServiceMock;
    private Mock<ICsvParsingService<FundCsvRow>> _csvParsingServiceMock;
    private Mock<IDownloaderService> _downloaderServiceMock;
    private Mock<ILoggingService> _loggerMock;
    private Mock<IConfiguration> _configurationMock;
    private Mock<IConfigurationSection> _configurationSectionMock;
    private IFundCsvService _fundCsvService;

    [SetUp]
    public void SetUp()
    {
        // Mock IConfigurationSection
        _configurationSectionMock = new Mock<IConfigurationSection>();
        _configurationSectionMock.SetupGet(m => m.Value).Returns(Url);

        // Mock IConfiguration
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(m => m.GetSection(It.IsAny<string>())).Returns(_configurationSectionMock.Object);

        // Mock other dependencies
        _holdingServiceMock = new Mock<IHoldingService>();
        _csvParsingServiceMock = new Mock<ICsvParsingService<FundCsvRow>>();
        _downloaderServiceMock = new Mock<IDownloaderService>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILoggingService>();

        // Initialize FundCsvService with mocked dependencies
        _fundCsvService = new FundCsvService(
            _holdingServiceMock.Object,
            _csvParsingServiceMock.Object,
            _downloaderServiceMock.Object,
            unitOfWorkMock.Object,
            _loggerMock.Object,
            _configurationMock.Object
        );
    }

    [Test]
    public async Task UpdateHoldings_SuccessfulUpdate_ReturnsCorrectCount()
    {
        // Arrange
        const string csvString = "Fund,Cusip,Ticker,Company,Shares,MarketValue,Weight,Date\n" +
                                 "ARKK,12345,ABC,Test Company,1000,$10000,10%,01/01/2022";
        _downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(csvString);
        _csvParsingServiceMock.Setup(m => m.ParseString(It.IsAny<string>(), It.IsAny<CancellationToken>()))
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
        _holdingServiceMock.Setup(m => m.AddHolding(It.IsAny<AddHoldingDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new HoldingDTO()));

        // Act
        var result = await _fundCsvService.UpdateHoldings();

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }

    [Test]
    public void UpdateHoldings_FailToDownload_ThrowsException()
    {
        // Arrange
        _downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string)null!);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _fundCsvService.UpdateHoldings());
    }

    [Test]
    public void UpdateHoldings_FailToParse_ThrowsException()
    {
        // Arrange
        const string csvString = "Fund,Cusip,Ticker,Company,Shares,MarketValue,Weight,Date\n" +
                                 "ARKK,12345,ABC,Test Company,1000,$10000,10%,01/01/2022";
        _downloaderServiceMock.Setup(m => m.DownloadTextFileAsStringAsync(It.IsAny<string>(),
                It.IsAny<List<(string, string)>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(csvString);
        _csvParsingServiceMock.Setup(m => m.ParseString(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns((List<FundCsvRow>)null!);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(() => _fundCsvService.UpdateHoldings());
    }

    [Test]
    public void UpdateHoldings_NoCsvUrl_ThrowsException()
    {
        // Arrange
        _configurationSectionMock.SetupGet(m => m.Value).Returns((string)null!);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _fundCsvService.UpdateHoldings());
    }
}