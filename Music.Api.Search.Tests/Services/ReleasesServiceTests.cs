using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using System.Net.Http;
using Moq.Contrib.HttpClient;
using Music.Api.Search.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using Music.Api.Search.Models;
using Newtonsoft.Json;
using Music.Api.Search.MusicBrainz;
using AutoFixture;

namespace Music.Api.Search.Tests.Services
{
    public class ReleasesServiceTests
    {
        [Fact]
        public async void ReleasesServiceReturnsReleasesCollection()
        {
            var fixture = new Fixture();
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            var testResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(GetTestResponse()))
            };
            httpMessageHandler.SetupAnyRequest().Returns(Task.FromResult(testResponse));
            
            var httpClientFactory = httpMessageHandler.CreateClientFactory();
            Mock.Get(httpClientFactory).Setup(x => x.CreateClient("ReleasesService"))
                .Returns(() =>
                {
                    var client = httpMessageHandler.CreateClient();
                    client.BaseAddress = fixture.Create<Uri>();
                    return client;
                });

            var loggerMock = new Mock<ILogger<ReleasesService>>();

            var service = new ReleasesService(httpClientFactory, loggerMock.Object);
            var result = await service.GetReleaseAsync("1");
            Assert.Single(result);
        }

        [Fact]
        public async void ReleasesServiceReturnsNullOnError()
        {
            var fixture = new Fixture();
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            var testResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("")
            };
            httpMessageHandler.SetupAnyRequest().Returns(Task.FromResult(testResponse));

            var httpClientFactory = httpMessageHandler.CreateClientFactory();
            Mock.Get(httpClientFactory).Setup(x => x.CreateClient("ReleasesService"))
                .Returns(() =>
                {
                    var client = httpMessageHandler.CreateClient();
                    client.BaseAddress = fixture.Create<Uri>();
                    return client;
                });

            var loggerMock = new Mock<ILogger<ReleasesService>>();
            
            var service = new ReleasesService(httpClientFactory, loggerMock.Object);
            var result = await service.GetReleaseAsync("1");
            Assert.Null(result);
            Assert.Equal(1, loggerMock.Invocations.Count);
        }

        private ReleaseSearchResults GetTestResponse()
        {
            var releases = new List<Release>();
            releases.Add(new Release
            {
                Id = "123",
                Country = "AU",
                Title = "Release 1",
                Quality = "normal",
                Status = "Official",
                Disambiguation = "",
                Date = new DateTime()
            });
            return new ReleaseSearchResults {
                Releases = releases
            };
        }
    }
}
