using Music.Api.Search.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Music.Api.Search.Interfaces
{
    public interface IArtistsService
    {
        Task<IEnumerable<Artist>> SearchArtistsAsync(string artistName);
    }
}
