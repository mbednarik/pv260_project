using System.Net;

using FundParser.BL.Services.DownloaderService;
using FundParser.BL.Services.LoggingService;
using FundParser.BL.Tests.Mocks;

using Moq;

using NUnit.Framework.Internal;

namespace FundParser.BL.Tests.Services
{
    [TestFixture]
    public class DownloaderServiceTests
    {
        private const string UrlValid = "https://www.seznam.cz/";
        private readonly List<(string, string)> _validHeaders = [("Accept", "text/csv")];
        private Mock<IMockHttpMessageHandler> _messageHandlerMock;
        private Mock<ILoggingService> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILoggingService>();
            _messageHandlerMock = new Mock<IMockHttpMessageHandler>();
        }

        [Test]
        public async Task DownloadTextFileAsString_InvalidLink_ReturnsNull()
        {
            // Setup
            var urlInvalid = "invalid url";
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var result = await downloaderService.DownloadTextFileAsStringAsync(urlInvalid, _validHeaders, CancellationToken.None);

            //Asserts
            Assert.That(result, Is.Null);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_InvalidHeader_ReturnsNull()
        {
            // Setup
            var invalidHeaders = new List<(string, string)> { ("Content-Type", "text/csv") };
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var result = await downloaderService.DownloadTextFileAsStringAsync(UrlValid, invalidHeaders, CancellationToken.None);

            //Asserts
            Assert.That(result, Is.Null);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_BadResponse_ReturnsNull()
        {
            // Setup
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var result = await downloaderService.DownloadTextFileAsStringAsync(UrlValid, _validHeaders, CancellationToken.None);

            //Asserts
            Assert.That(result, Is.EqualTo(null));
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_SendAsyncThrowsException_ReturnsNull()
        {
            // Setup
            var exceptionMessage = "Error occured when sending request";
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Throws(new HttpRequestException(exceptionMessage));
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var result = await downloaderService.DownloadTextFileAsStringAsync(UrlValid, _validHeaders, CancellationToken.None);

            //Asserts
            Assert.That(result, Is.Null);
            _loggerMock.Verify(logger => logger.LogError($"Unable to download file from {UrlValid}, error: {exceptionMessage}",
                It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_Valid_ReturnsCorrectString()
        {
            // Setup
            var testString = "test, test2, test3";
            var request = new HttpRequestMessage(HttpMethod.Get, UrlValid);
            foreach (var header in _validHeaders)
            {
                request.Headers.Add(header.Item1, header.Item2);
            }
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(testString)
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var result = await downloaderService.DownloadTextFileAsStringAsync(UrlValid, _validHeaders, CancellationToken.None);

            // Assert
            Assert.That(result, Is.EqualTo(testString));
            _messageHandlerMock.Verify(mockMessageHandler => mockMessageHandler.SendAsync(It.Is<HttpRequestMessage>(actualRequest =>
                actualRequest.Method == HttpMethod.Get &&
                actualRequest.RequestUri == new Uri(UrlValid)
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}