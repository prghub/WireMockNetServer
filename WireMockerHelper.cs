using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace WireMockNetServer
{
    public class WireMockServerHelper : IDisposable
    {
        private WireMockServer _server;

        public WireMockServerHelper(string url = "http://localhost:8080")
        {
            _server = WireMockServer.Start(new WireMockServerSettings
            {
                Urls = new[] { url }
            });
        }

        public WireMockServer Server => _server;

        // Helper method to set up a GET request with a specific response
        public void SetupGetRequest(string path, object responseBody, int statusCode = 200)
        {
            _server.Given(Request.Create().WithPath(path).UsingGet())
                .RespondWith(Response.Create().WithStatusCode(statusCode).WithBodyAsJson(responseBody));
        }

        // Helper method to set up a GET request with a delay
        public void SetupGetRequestWithDelay(string path, string responseBody, TimeSpan delay, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            _server.Given(Request.Create().WithPath(path).UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(statusCode)
                    .WithBody(responseBody)
                    .WithDelay(delay));
        }

        public void SetupGetRequestWithHeader(string path, Dictionary<string, string> headers, object responseBody, int statusCode = 200)
        {
            var requestBuilder = Request.Create().WithPath(path).UsingGet();

            // Add headers dynamically if they are provided
                foreach (var header in headers)
                {
                    requestBuilder.WithHeader(header.Key, header.Value);
                }

            _server.Given(requestBuilder)
                .RespondWith(Response.Create().WithStatusCode(statusCode).WithBodyAsJson(responseBody));
        }

        // Helper method to set up a POST request with a specific response
        public void SetupPostRequest(string path, object responseBody, int statusCode = 200)
        {
            _server.Given(Request.Create().WithPath(path).UsingPost())
                .RespondWith(Response.Create().WithStatusCode(statusCode).WithBodyAsJson(responseBody));
        }

        public void Dispose()
        {
            _server?.Stop();
            _server?.Dispose();
        }
    }
}
