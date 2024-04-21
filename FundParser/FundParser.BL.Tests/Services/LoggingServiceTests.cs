using FundParser.BL.Services.LoggingService;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moq;

namespace FundParser.BL.Tests.Services;

[TestFixture]
public class LoggingServiceTests
{
    public class ConstructorTests : LoggingServiceTestsUninitializedBase
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
            // Initialize LoggingService with mocked IConfiguration
            _ = new LoggingService(mockConfigurationMock.Object);

            // Arrange & Act & Assert
            Assert.That(Directory.Exists(testLogFolderPath), Is.True);
        }
    }

    public class LogInformationTests : LoggingServiceTestsInitializedBase
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

            AssertLogLine(logFileLastLine, message, source, LogLevel.Information);
        }
    }

    public class LogWarningTests : LoggingServiceTestsInitializedBase
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

            AssertLogLine(logFileLastLine, message, source, LogLevel.Warning);
        }
    }

    public class LogErrorTests : LoggingServiceTestsInitializedBase
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

            AssertLogLine(logFileLastLine, message, source, LogLevel.Error);
        }
    }

    [TestFixture]
    public class LoggingServiceTestsUninitializedBase
    {
        protected string testLogFolderPath;
        protected Mock<IConfiguration> mockConfigurationMock;
        protected Mock<IConfigurationSection> mockConfigurationSectionMock;

        [SetUp]
        public void BaseSetUp()
        {
            testLogFolderPath = Path.Combine(Path.GetTempPath(), "TestLogs");

            // Mock IConfigurationSection
            mockConfigurationSectionMock = new Mock<IConfigurationSection>();
            mockConfigurationSectionMock.SetupGet(m => m.Value).Returns(testLogFolderPath);

            // Mock IConfiguration
            mockConfigurationMock = new Mock<IConfiguration>();
            mockConfigurationMock.Setup(m => m.GetSection(It.IsAny<string>()))
                .Returns(mockConfigurationSectionMock.Object);
        }

        [TearDown]
        public void BaseTearDown()
        {
            if (Directory.Exists(testLogFolderPath))
            {
                Directory.Delete(testLogFolderPath, true);
            }
        }

        protected static void AssertLogLine(string logLine, string message, string source, LogLevel logLevel)
        {
            Assert.Multiple(() =>
            {
                Assert.That(logLine, Does.Contain(message));
                Assert.That(logLine, Does.Contain(source));
                Assert.That(logLine, Does.Contain(logLevel.ToString()));
                Assert.That(logLine, Does.EndWith($"{logLevel}: {source}: {message}"));
            });
        }
    }

    public class LoggingServiceTestsInitializedBase : LoggingServiceTestsUninitializedBase
    {
        protected LoggingService loggingService;

        [SetUp]
        public void SetUp()
        {
            loggingService = new LoggingService(mockConfigurationMock.Object);
        }
    }
}