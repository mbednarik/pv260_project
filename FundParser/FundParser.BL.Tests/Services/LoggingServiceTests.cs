using FundParser.BL.Services.LoggingService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FundParser.BL.Tests.Services;

[TestFixture]
public class LoggingServiceTests
{
    private Mock<IConfiguration> _mockConfiguration;
    private Mock<IConfigurationSection> _mockConfigurationSection;
    private string _testLogFolderPath = Path.Combine(Path.GetTempPath(), "TestLogs");
    private ILoggingService _loggingService;

    [SetUp]
    public void SetUp()
    {
        // Mock IConfigurationSection
        _mockConfigurationSection = new Mock<IConfigurationSection>();
        _mockConfigurationSection.SetupGet(m => m.Value).Returns(_testLogFolderPath);

        // Mock IConfiguration
        _mockConfiguration = new Mock<IConfiguration>();
        _mockConfiguration.Setup(m => m.GetSection(It.IsAny<string>())).Returns(_mockConfigurationSection.Object);

        // Initialize LoggingService with mocked IConfiguration
        _loggingService = new LoggingService(_mockConfiguration.Object);
    }


    [Test]
    public void Constructor_ValidConfiguration_CreatesLogFolder()
    {
        // Arrange & Act & Assert
        Assert.IsTrue(Directory.Exists(_testLogFolderPath));
    }

    [Test]
    public void Constructor_InvalidConfiguration_ThrowsException()
    {
        // Arrange
        _mockConfigurationSection.SetupGet(m => m.Value).Returns((string)null);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => new LoggingService(_mockConfiguration.Object));
    }

    [Test]
    public async Task LogInformation_ValidMessage_WritesToLogFile()
    {
        // Arrange
        var message = "Test information message";
        var source = "Test information source";

        // Act
        await _loggingService.LogInformation(message, source);

        // Assert
        var logFilePath = Path.Combine(_testLogFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");

        var logFileLines = await File.ReadAllLinesAsync(logFilePath);
        var logFileLastLine = logFileLines.Last();
        Assert.Multiple(() =>
        {
            Assert.That(logFileLastLine, Does.Contain(message));
            Assert.That(logFileLastLine, Does.Contain(source));
            Assert.That(logFileLastLine, Does.Contain(LogLevel.Information.ToString()));
            Assert.That(logFileLastLine, Does.EndWith($"{LogLevel.Information}: {source}: {message}"));
        });
    }

    [Test]
    public async Task LogWarning_ValidMessage_WritesToLogFile()
    {
        // Arrange
        var message = "Test warning message";
        var source = "Test warning source";

        // Act
        await _loggingService.LogWarning(message, source);

        // Assert
        var logFilePath = Path.Combine(_testLogFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");

        var logFileLines = await File.ReadAllLinesAsync(logFilePath);
        var logFileLastLine = logFileLines.Last();
        Assert.Multiple(() =>
        {
            Assert.That(logFileLastLine, Does.Contain(message));
            Assert.That(logFileLastLine, Does.Contain(source));
            Assert.That(logFileLastLine, Does.Contain(LogLevel.Warning.ToString()));
            Assert.That(logFileLastLine, Does.EndWith($"{LogLevel.Warning}: {source}: {message}"));
        });
    }

    [Test]
    public async Task LogError_ValidMessage_WritesToLogFile()
    {
        // Arrange
        var message = "Test error message";
        var source = "Test error source";

        // Act
        await _loggingService.LogError(message, source);

        // Assert
        var logFilePath = Path.Combine(_testLogFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");

        var logFileLines = await File.ReadAllLinesAsync(logFilePath);
        var logFileLastLine = logFileLines.Last();
        Assert.Multiple(() =>
        {
            Assert.That(logFileLastLine, Does.Contain(message));
            Assert.That(logFileLastLine, Does.Contain(source));
            Assert.That(logFileLastLine, Does.Contain(LogLevel.Error.ToString()));
            Assert.That(logFileLastLine, Does.EndWith($"{LogLevel.Error}: {source}: {message}"));
        });
    }
    
    [TearDown]
    public void TearDown()
    {
        var logFilePath = Path.Combine(_testLogFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }
}