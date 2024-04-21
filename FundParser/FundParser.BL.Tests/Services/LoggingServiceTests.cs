using FundParser.BL.Services.LoggingService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace FundParser.BL.Tests.Services;

[TestFixture]
public class LoggingServiceTests
{
    public class ConstructorTests : LoggingServiceTestsBase
    {
        [Test]
        public void Constructor_InvalidConfiguration_ThrowsException()
        {
            // Arrange
            mockConfigurationSectionMock.SetupGet(m => m.Value).Returns((string)null!);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => new LoggingService(mockConfigurationMock.Object));
        }
        
        [Test]
        public void Constructor_ValidConfiguration_CreatesLogFolder()
        {
            // Arrange & Act & Assert
            Assert.That(Directory.Exists(testLogFolderPath), Is.True);
        }
    }

    public class LogInformationTests : LoggingServiceTestsBase
    {
        [Test]
        public async Task LogInformation_ValidMessage_WritesToLogFile()
        {
            // Arrange
            const string message = "Test information message";
            const string source = "Test information source";

            // Act
            await loggingService.LogInformation(message, source);

            // Assert
            var logFilePath = Path.Combine(testLogFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");

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
    }

    public class LogWarningTests : LoggingServiceTestsBase
    {
        [Test]
        public async Task LogWarning_ValidMessage_WritesToLogFile()
        {
            // Arrange
            const string message = "Test warning message";
            const string source = "Test warning source";

            // Act
            await loggingService.LogWarning(message, source);

            // Assert
            var logFilePath = Path.Combine(testLogFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");

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
    }

    public class LogErrorTests : LoggingServiceTestsBase
    {
        [Test]
        public async Task LogError_ValidMessage_WritesToLogFile()
        {
            // Arrange
            const string message = "Test error message";
            const string source = "Test error source";

            // Act
            await loggingService.LogError(message, source);

            // Assert
            var logFilePath = Path.Combine(testLogFolderPath, $"{DateTime.Now:yyyy-MM-dd}.log");

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
    }


    public class LoggingServiceTestsBase
    {
        protected string testLogFolderPath;
        protected Mock<IConfiguration> mockConfigurationMock;
        protected Mock<IConfigurationSection> mockConfigurationSectionMock;
        protected LoggingService loggingService;

        [SetUp]
        public void SetUp()
        {
            testLogFolderPath = Path.Combine(Path.GetTempPath(), "TestLogs");

            // Mock IConfigurationSection
            mockConfigurationSectionMock = new Mock<IConfigurationSection>();
            mockConfigurationSectionMock.SetupGet(m => m.Value).Returns(testLogFolderPath);

            // Mock IConfiguration
            mockConfigurationMock = new Mock<IConfiguration>();
            mockConfigurationMock.Setup(m => m.GetSection(It.IsAny<string>()))
                .Returns(mockConfigurationSectionMock.Object);

            // Initialize LoggingService with mocked IConfiguration
            loggingService = new LoggingService(mockConfigurationMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(testLogFolderPath))
            {
                Directory.Delete(testLogFolderPath, true);
            }
        }
    }
}