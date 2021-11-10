using System;
using Xunit;
using Moq;
using Music.Api.Search.Interfaces;
using Music.Api.Search.Services;
using System.Collections.Generic;
using Music.Api.Search.Models;
using System.Linq;

namespace Music.Api.Search.Tests
{
    public class SearchServiceTests
    {
        [Fact]
        public async void SearchServiceReturnsSingleArtistWithReleases()
        {
            var searchTerm = "Artist";
            var artistId = "1";
            var count = 1;
            var mockArtistService = new Mock<IArtistsService>();
            var mockReleaseService = new Mock<IReleasesService>();

            mockArtistService.Setup(service => service.SearchArtistsAsync(searchTerm))
                .ReturnsAsync(GetArtists(count));

            mockReleaseService.Setup(service => service.GetReleaseAsync(artistId))
                .ReturnsAsync(GetReleases(count));
            
            var searchService = new SearchService(mockArtistService.Object, mockReleaseService.Object);
            var result = await searchService.SearchArtistsAsync(searchTerm);
            Assert.Empty(result.Message);
            Assert.Single(result.SearchResults);
            Assert.Single(result.SearchResults.First().Releases);
        }

        [Fact]
        public async void SearchServiceReturnsDefaultResponseForSingleArtistOnError()
        {
            var searchTerm = "Artist";
            var artistId = "1";
            var count = 1;
            var mockArtistService = new Mock<IArtistsService>();
            var mockReleaseService = new Mock<IReleasesService>();

            mockArtistService.Setup(service => service.SearchArtistsAsync(searchTerm))
                .ReturnsAsync(GetArtists(count));

            mockReleaseService.Setup(service => service.GetReleaseAsync(artistId))
                .ReturnsAsync(null as IEnumerable<Release>);

            var searchService = new SearchService(mockArtistService.Object, mockReleaseService.Object);
            var result = await searchService.SearchArtistsAsync(searchTerm);
            Assert.NotEmpty(result.Message);
            Assert.Single(result.SearchResults);
            Assert.Null(result.SearchResults.First().Releases);
        }

        [Fact]
        public async void SearchServiceReturnsArtistsCollection()
        {
            var searchTerm = "Artist";
            var artistId = "123";
            var count = 3;
            var mockArtistService = new Mock<IArtistsService>();
            var mockReleaseService = new Mock<IReleasesService>();

            mockArtistService.Setup(service => service.SearchArtistsAsync(searchTerm))
                .ReturnsAsync(GetArtists(count));

            mockReleaseService.Setup(service => service.GetReleaseAsync(artistId))
                .ReturnsAsync(GetReleases());

            var searchService = new SearchService(mockArtistService.Object, mockReleaseService.Object);
            var result = await searchService.SearchArtistsAsync(searchTerm);
            Assert.Empty(result.Message);
            Assert.Equal(count, result.SearchResults.Count());
        }

        [Fact]
        public async void SearchServiceReturnsMessageOnError()
        {
            var searchTerm = "Artist";
            var artistId = "123";
            var mockArtistService = new Mock<IArtistsService>();
            var mockReleaseService = new Mock<IReleasesService>();

            mockArtistService.Setup(service => service.SearchArtistsAsync(searchTerm))
                .ReturnsAsync(null as IEnumerable<Artist>);

            mockReleaseService.Setup(service => service.GetReleaseAsync(artistId))
                .ReturnsAsync(GetReleases());

            var searchService = new SearchService(mockArtistService.Object, mockReleaseService.Object);
            var result = await searchService.SearchArtistsAsync(searchTerm);
            Assert.NotEmpty(result.Message);
            Assert.Null(result.SearchResults);
        }

        private List<Artist> GetArtists(int count)
        {
            var list = new List<Artist>();
            for (var i = 1; i <= count; i++)
            {
                list.Add(new Artist
                {
                    Id = $"{i}",
                    Country = "AU",
                    Name = $"Artist {i}",
                    Releases = null
                });
            }
            return list;
        }

        private List<Release> GetReleases(int count = 1)
        {
            var date = new DateTime();
            var list = new List<Release>();
            for (var i = 1; i <= count; i++)
            {
                list.Add(new Release
                {
                    Id = $"{i}",
                    Country = "AU",
                    Title = $"Release {i}",
                    Quality = "normal",
                    Status = "Official",
                    Disambiguation = "",
                    Date = date
                });
            }
            return list;
        }
    }
}
