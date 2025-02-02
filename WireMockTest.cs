using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit.Abstractions;

namespace WireMockNetServer
{
    public class WireMockTest : BaseTest
    {
        private readonly ITestOutputHelper output;

        // Constructor injection of ITestOutputHelper
        public WireMockTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task PostSimulatingFailure()
        {
            // Arrange: Set up the POST request with a simulated failure (500 Internal Server Error)
            WireMockHelper.SetupPostRequest("/api/Payment", new { error = "Internal Server Error" }, 500);
            
           // Act: Make a POST request to the /api/Payment endpoint
            var content = new StringContent("{ \"paymentAmount\": 100, \"paymentMethod\": \"CreditCard\" }", Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync("/api/Payment", content);

           // Assert: Verify that the response status code is 500 (Internal Server Error)
           Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);

           // Assert: Verify that the response body contains the expected error message
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("\"error\":\"Internal Server Error\"", responseContent);
        }

        [Fact]
        public async Task GetWithQueryParameters()
        {
            // Arrange: Set up the GET request with query parameters and a response body
            WireMockHelper.SetupGetRequest("/api/user", new
            {
                id = "123",
                name = "John Doe"
            });
            // Act: Make a GET request using the inherited HttpClient
            var response = await HttpClient.GetAsync("/api/user?id=123");

            // Assert: Verify that the response status code is 200 OK
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            // Assert: Read the response content and verify that it contains the expected values
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response to a dynamic object for easy property access
            var user = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Assert: Check if the id and name match the expected values
            Assert.Equal("123", user.id.ToString());
            Assert.Equal("John Doe", user.name.ToString());

            // Optional: Check if the response contains the expected structure (id and name fields)
            Assert.Contains("id", responseContent);
            Assert.Contains("name", responseContent);

        }

        [Fact]
        public async Task GetSimulatingSlowResponse()
        {
            // Arrange: Use the helper to set up the mock GET request with a delay
            WireMockHelper.SetupGetRequestWithDelay("/api/data", "{\"data\":\"Some data\"}", TimeSpan.FromSeconds(5));

            // Act: Make a GET request
            var response = await HttpClient.GetAsync("/api/data");

            // Assert: Verify that the response status code is 200 OK
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            // Assert: Read the response content and verify that it contains the expected data
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Some data", responseContent);
        }

        [Fact]
        public async Task GetWithAuthorizationHeader()
        {
            // Arrange: Set up the mock response for GET request with Authorization header
            var headers = new Dictionary<string, string>
            {
                { "Authorization", "Bearer some-token" }
            };
            WireMockHelper.SetupGetRequestWithHeader("/api/user", headers, new { name = "John Doe" });

            // Log the WireMock server URL using ITestOutputHelper
            output.WriteLine($"WireMock server started at {WireMockHelper.Server.Url}");

            // Act: Make a GET request using the inherited HttpClient
            var response = await HttpClient.GetAsync("/api/user");

            // Assert: Verify that the response status code is 200 OK
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            // Assert: Read the response content and verify that it contains the expected values
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response to a dynamic object for easy property access
            var user = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Assert: Verify that the 'name' is "John Doe"
            Assert.Equal("John Doe", user.name.ToString());
        }

    }
}
