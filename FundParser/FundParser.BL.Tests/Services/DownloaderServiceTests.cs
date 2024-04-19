using FundParser.BL.Services.DownloaderService;
using FundParser.BL.Services.LoggingService;
using FundParser.BL.Tests.Mocks;
using Moq;
using NUnit.Framework.Internal;
using System;
using System.Net;

namespace FundParser.BL.Tests.Services
{
    [TestFixture]
    public class DownloaderServiceTests
    {
        private const string URL_VALID = "https://www.seznam.cz/";
        private readonly List<(string, string)> VALID_HEADERS = [("Accept", "text/csv")];
        private Mock<IMockHttpMessageHandler> _mockMessageHandler;
        private Mock<ILoggingService> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILoggingService>();
            _mockMessageHandler = new Mock<IMockHttpMessageHandler>();
        }

        [Test]
        public async Task DownloadTextFileAsString_CorrectUse_Success()
        {
            // Setup
            var testString = "test, test2, test3";
            var request = new HttpRequestMessage(HttpMethod.Get, URL_VALID);
            foreach (var header in VALID_HEADERS)
            {
                request.Headers.Add(header.Item1, header.Item2);
            }
            _mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(testString)
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_mockMessageHandler.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(URL_VALID, VALID_HEADERS, new CancellationToken());

            // Assert
            Assert.That(csvString, Is.EqualTo(testString));
            _mockMessageHandler.Verify(mockMessageHandler => mockMessageHandler.SendAsync(It.Is<HttpRequestMessage>(actualRequest =>
                actualRequest.Method == HttpMethod.Get &&
                actualRequest.RequestUri == new Uri(URL_VALID)
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_InvalidLink_ReturnsNull()
        {
            // Setup
            string urlInvalid = "invalid url";
            _mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_mockMessageHandler.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(urlInvalid, VALID_HEADERS, new CancellationToken());

            //Asserts
            Assert.That(csvString, Is.EqualTo(null));
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_InvalidHeader_ReturnsNull()
        {
            // Setup
            var invalidHeaders = new List<(string, string)> { ("Content-Type", "text/csv") };
            _mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_mockMessageHandler.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(URL_VALID, invalidHeaders, new CancellationToken());

            //Asserts
            Assert.That(csvString, Is.EqualTo(null));
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_BadResponse_ReturnsNull()
        {
            // Setup
            _mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = null
                });
            var httpClient = new HttpClient(new MockHttpMessageHandler(_mockMessageHandler.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(URL_VALID, VALID_HEADERS, new CancellationToken());

            //Asserts
            Assert.That(csvString, Is.EqualTo(null));
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task DownloadTextFileAsString_SendAsyncThrowsException_ReturnsNull()
        {
            // Setup
            var exceptionMessage = "Error occured when sending request";
            _mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .Throws(new HttpRequestException(exceptionMessage));
            var httpClient = new HttpClient(new MockHttpMessageHandler(_mockMessageHandler.Object));

            // Act
            var downloaderService = new DownloaderService(_loggerMock.Object, httpClient);
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(URL_VALID, VALID_HEADERS, new CancellationToken());

            //Asserts
            Assert.That(csvString, Is.EqualTo(null));
            _loggerMock.Verify(logger => logger.LogError($"Unable to download file from {URL_VALID}, error: {exceptionMessage}",
                It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}