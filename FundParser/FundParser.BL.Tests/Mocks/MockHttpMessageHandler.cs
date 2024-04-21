using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundParser.BL.Tests.Mocks
{
    public interface IMockHttpMessageHandler
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly IMockHttpMessageHandler _mockHandler;

        public MockHttpMessageHandler(IMockHttpMessageHandler mockHandler)
        {
            _mockHandler = mockHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await _mockHandler.SendAsync(request, cancellationToken);
        }
    }
}
