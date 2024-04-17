using FundParser.BL.Services.DownloaderService;
using FundParser.BL.Services.LoggingService;
using FundParser.BL.Tests.Helpers;
using Moq;
using NUnit.Framework.Internal;

namespace FundParser.BL.Tests
{
    public class DownloaderTests
    {
        private const string urlValid = "https://www.seznam.cz/";
        private const string urlInvalid = "invalid url";
        private const string testString = "test, test2, test3";
        private Mock<ILoggingService> logger;
        private readonly List<(string, string)> validHeaders = [("Accept", "text/csv")];
        private readonly List<(string, string)> invalidHeaders = [("Content-Type", "text/csv")];
        private Mock<IMockHttpMessageHandler> mockMessageHandler;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILoggingService>();
            mockMessageHandler = new Mock<IMockHttpMessageHandler>();
        }

        [Test]
        public async Task DownloaderService_DownloadTextFile_Success()
        {
            // Setup
            HttpRequestMessage request = new(HttpMethod.Get, urlValid);
            foreach (var header in validHeaders)
            {
                request.Headers.Add(header.Item1, header.Item2);
            }
            mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(testString)
                });

            // Act
            var downloaderService = new DownloaderService(logger.Object, new MockHttpMessageHandler(mockMessageHandler.Object));
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(urlValid, validHeaders, new CancellationToken());

            // Assert
            Assert.That(csvString, Is.EqualTo(testString));
            try
            {
                mockMessageHandler.Verify(mockMessageHandler => mockMessageHandler.SendAsync(It.Is<HttpRequestMessage>(actualRequest =>
                    actualRequest.Method == HttpMethod.Get &&
                    actualRequest.RequestUri == new Uri(urlValid)
                ), It.IsAny<CancellationToken>()), Times.Once);
            }
            catch (MockException e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public async Task DownloaderService_DownloadTextFile_InvalidLink()
        {
            // Setup
            mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = null
                });

            //Act
            var downloaderService = new DownloaderService(logger.Object, new MockHttpMessageHandler(mockMessageHandler.Object));
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(urlInvalid, validHeaders, new CancellationToken());

            //Asserts
            Assert.That(csvString, Is.EqualTo(null));
            try
            {
                logger.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            } 
            catch (MockException e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public async Task DownloaderService_DownloadTextFile_InvalidHeader()
        {
            // Setup
            mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = null
                });

            //Act
            var downloaderService = new DownloaderService(logger.Object, new MockHttpMessageHandler(mockMessageHandler.Object));
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(urlValid, invalidHeaders, new CancellationToken());

            //Asserts
            Assert.That(csvString, Is.EqualTo(null));
            try
            {
                logger.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            } 
            catch (MockException e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public async Task DownloaderService_DownloadTextFile_BadResponse()
        {
            // Setup
            mockMessageHandler.Setup(Setup => Setup.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = null
                });

            //Act
            var downloaderService = new DownloaderService(logger.Object, new MockHttpMessageHandler(mockMessageHandler.Object));
            var csvString = await downloaderService.DownloadTextFileAsStringAsync(urlValid, validHeaders, new CancellationToken());

            //Asserts
            Assert.That(csvString, Is.EqualTo(null));
            try
            {
                logger.Verify(logger => logger.LogError(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            } 
            catch (MockException e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}