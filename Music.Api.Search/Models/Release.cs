using System;

namespace Music.Api.Search.Models
{
    public class Release
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string Quality { get; set; }
        public string Status { get; set; }
        public string Disambiguation { get; set; }
        public DateTime Date { get; set; }
    }
}
