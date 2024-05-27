using System.Net;

using FundParser.BL.Exceptions;
using FundParser.BL.Services.DownloaderService;
using FundParser.BL.Services.LoggingService;
using FundParser.BL.Tests.Mocks;

using Moq;

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
        public void DownloadTextFileAsString_InvalidLink_ThrowsException()
        {
            // Setup
            var urlInvalid = "invalid url";
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);

            // Act, Assert
            Assert.That(async () => await downloaderService.DownloadTextFileAsStringAsync(urlInvalid, _validHeaders, CancellationToken.None), Throws
                .TypeOf<UriFormatException>()
                .With
                .Message
                .EqualTo("Invalid URI: The format of the URI could not be determined."));
        }

        [Test]
        public void DownloadTextFileAsString_InvalidHeader_ThrowsException()
        {
            // Setup
            var invalidHeaders = new List<(string, string)> { ("Content-Type", "text/csv") };
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);

            // Act, Assert
            Assert.That(async () =>
                await downloaderService.DownloadTextFileAsStringAsync(UrlValid, invalidHeaders, CancellationToken.None), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void DownloadTextFileAsString_BadResponse_ThrowsException()
        {
            // Setup
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);

            // Act, Assert
            Assert.That(async () =>
                await downloaderService.DownloadTextFileAsStringAsync(UrlValid, _validHeaders, CancellationToken.None), Throws
                .TypeOf<ApiErrorException>()
                .With
                .Message
                .EqualTo($"Failed to download csv, server responded with status code: {HttpStatusCode.BadRequest}"));

            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void DownloadTextFileAsString_SendAsyncThrowsException_ThrowsException()
        {
            // Setup
            var exceptionMessage = "Error occurred when sending request";
            _messageHandlerMock.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Throws(new HttpRequestException(exceptionMessage));
            var httpClient = new HttpClient(new MockHttpMessageHandler(_messageHandlerMock.Object));
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);

            // Act, Assert
            Assert.That(async () => await downloaderService.DownloadTextFileAsStringAsync(UrlValid, _validHeaders, CancellationToken.None), Throws
                .TypeOf<HttpRequestException>()
                .With
                .Message
                .EqualTo(exceptionMessage));
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
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);

            // Act
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
