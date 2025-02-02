# WireMock Testing Setup with Xunit and HttpClient

This repository demonstrates how to mock external API responses using **WireMock** in combination with **XUnit** for unit testing. It leverages **WireMock.Net** for creating mock HTTP servers and **HttpClient** for making API requests within the tests.

## Setup

### Mock Server Initialization
The mock server is initialized using **WireMock.Net**, which allows for setting up specific request/response configurations. The mock server listens on a predefined base URL (`http://localhost:8080`), and each test configures the necessary endpoints and response behaviors.

### Test Structure

- **BaseTest Class**: This class initializes the `WireMockServerHelper` and `HttpClient`, which can be used by all tests. It also ensures that the resources are disposed of properly after each test.

- **WireMockTest Class**: Contains individual test methods that demonstrate how to simulate different behaviors, such as failure, slow response, and header-based authorization.

### Example Tests

### 1. Simulating a POST Request Failure
Test the behavior of your application when a POST request results in a server error. This is useful for simulating scenarios where the external API is down or returns an error.
- **Simulating Failure**: A POST request that simulates a server error (500) is tested to ensure the application handles it gracefully.


### 2. Handling GET Requests with Query Parameters
Mock and test a GET request with query parameters, verifying that the application handles the query parameters correctly and that the response body contains the expected data.
- **GET with Query Parameters**: A GET request with query parameters (`id=123`) is tested to ensure the response contains the expected data (`id=123` and `name="John Doe"`).


### 3. Simulating Slow Response
Test how your application behaves when an API request takes longer than expected. The mock server can simulate a slow response using delays, helping to ensure your application can handle timeouts or delays appropriately.
- **Simulating Slow Response**: A GET request is tested with a delay, ensuring that the response is handled after the specified delay.


### 4. Authorization Header Simulation
Simulate API requests that require an authorization header, and verify that your application correctly handles authorization logic and responds with the expected data.
- **GET with Authorization Header**: A GET request with an `Authorization` header (`Bearer some-token`) is tested to simulate authenticated requests.

## Requirements

- **WireMock.Net**: Used for mocking HTTP endpoints.
- **XUnit**: Test framework for running the tests.
- **HttpClient**: For making HTTP requests to the mocked server during tests.
- **Newtonsoft.Json**: For serializing and deserializing JSON responses.

## Running Tests

1. Ensure **WireMock.Net** is set up and running on the base URL (`http://localhost:8080`).
2. Run the tests using **XUnit**.
3. The tests simulate various API scenarios (such as failures, query parameters, slow responses, and headers) and assert that the application behaves as expected.

## Conclusion

This setup is useful for testing scenarios where external services are simulated using **WireMock** to ensure that your application handles various response types, including successful responses, errors, delays, and authorization. With this approach, you can effectively test your application's interactions with external services without relying on those services being available during test execution.
