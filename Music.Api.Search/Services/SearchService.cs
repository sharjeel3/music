using Music.Api.Search.Interfaces;
using Music.Api.Search.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IArtistsService artistsService;
        private readonly IReleasesService releasesService;

        public SearchService(IArtistsService artistsService, IReleasesService releasesService)
        {
            this.artistsService = artistsService;
            this.releasesService = releasesService;
        }

        /// <summary>
        /// Search artits using an artist name
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns>Artists</returns>
        public async Task<(string Message, IEnumerable<Artist> SearchResults)> SearchArtistsAsync(string artistName)
        {
            var artistsResult = await artistsService.SearchArtistsAsync(artistName);

            if (artistsResult == null)
            {
                return ("Could not retrieve results", null);
            }
            if (artistsResult.Count().Equals(1))
            {
                var releases = await releasesService.GetReleaseAsync(artistsResult.First().Id);
                if (releases == null)
                {
                    return ("Could not retrieve releases information", artistsResult);
                }
                artistsResult.First().Releases = releases;
            }
            return ("", artistsResult);
        }
    }
}
