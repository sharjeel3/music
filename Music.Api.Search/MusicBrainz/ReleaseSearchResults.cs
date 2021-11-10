using Newtonsoft.Json;
using System.Collections.Generic;

namespace Music.Api.Search.MusicBrainz
{
    public class ReleaseSearchResults
    {
        [JsonProperty("releases")]
        public IEnumerable<Models.Release> Releases;
    }
}
