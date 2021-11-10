using System.Collections.Generic;

namespace Music.Api.Search.Models
{
    public class Artist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public IEnumerable<Release> Releases { get; set; }
    }
}
