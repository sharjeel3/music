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
    public class ArtistsService : IArtistsService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<ArtistsService> logger;

        public ArtistsService(IHttpClientFactory httpClientFactory, ILogger<ArtistsService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
        }

        /// <summary>
        /// Returns Artists using a search engine
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns>Artists</returns>
        public async Task<IEnumerable<Artist>> SearchArtistsAsync(string artistName)
        {
            try
            {
                var client = httpClientFactory.CreateClient("ArtistsService");
                // hard coding options for time being
                var artistsResponse = await client.GetAsync($"?fmt=json&limit=10&query={artistName}");
                var content = await artistsResponse.Content.ReadAsStringAsync();
                var result =  JsonConvert.DeserializeObject<MusicBrainz.ArtistSearchResults>(content);
                return result.Artists;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
