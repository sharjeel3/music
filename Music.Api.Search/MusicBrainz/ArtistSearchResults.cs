using Newtonsoft.Json;
using System.Collections.Generic;

namespace Music.Api.Search.MusicBrainz
{
    public class ArtistSearchResults
    {
        [JsonProperty("artists")]
        public IEnumerable<Models.Artist> Artists;
    }
}
