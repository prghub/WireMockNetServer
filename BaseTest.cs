using System;
using System.Net.Http;

namespace WireMockNetServer
{
    public class BaseTest : IDisposable
    {
        protected WireMockServerHelper WireMockHelper;
        protected HttpClient HttpClient;


        public BaseTest()
        {
            WireMockHelper = new WireMockServerHelper();

            // Initialize the HttpClient (with a base URL if necessary)
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080") // Adjust the base URL if needed
            };
        }

        public void Dispose()
        {
            WireMockHelper?.Dispose();
            HttpClient?.Dispose();

        }
    }

}
