using Microsoft.Extensions.Logging;
using Music.Api.Search.Interfaces;
using Music.Api.Search.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Music.Api.Search.Services
{
    public class ReleasesService : IReleasesService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ReleasesService> logger;

        public ReleasesService(IHttpClientFactory httpClientFactory, ILogger<ReleasesService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Returns Releases by an Artist Id
        /// </summary>
        /// <param name="artistId"></param>
        /// <returns>Releases</returns>
        public async Task<IEnumerable<Release>> GetReleaseAsync(string artistId)
        {
            try
            {
                var client = httpClientFactory.CreateClient("ReleasesService");
                // hard coding options for time being
                var releasesResponse = await client.GetAsync($"?fmt=json&limit=10&artist={artistId}");
                var content = await releasesResponse.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<MusicBrainz.ReleaseSearchResults>(content);
                return result.Releases;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
