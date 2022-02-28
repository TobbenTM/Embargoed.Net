using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using System.IO;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Embargoed.AspNetCore.Tests
{
    public class MiddlewareTests
    {
        private readonly HttpClient _client;

        public MiddlewareTests()
        {
            var testHost = new WebApplicationFactory<TestApp>()
                .WithWebHostBuilder(conf =>
                {
                    conf.UseContentRoot(Directory.GetCurrentDirectory());
                });
            _client = testHost.CreateClient();
        }

        [Fact]
        public async void GivenGoodOrigin_ReturnsDefaultPage()
        {
            // Arrange
            TestApp.RemoteIpAddress = IPAddress.Parse("5.2.64.0"); // See https://www.nirsoft.net/countryip/nl.html

            // Act
            var response = await _client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("This page is not embargoed", content);
        }

        [Fact]
        public async void GivenBadOrigin_ReturnsEmbargoedPage()
        {
            // Arrange
            TestApp.RemoteIpAddress = IPAddress.Parse("2.60.0.0"); // See https://www.nirsoft.net/countryip/ru.html

            // Act
            var response = await _client.GetAsync("/");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("We've blocked this website for Russian visitors.", content);
        }
    }
}
