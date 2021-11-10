using Music.Api.Search.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Music.Api.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(string Message, IEnumerable<Artist> SearchResults)> SearchArtistsAsync(string artistName);
    }
}
