using Music.Api.Search.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Music.Api.Search.Interfaces
{
    public interface IReleasesService
    {
        Task<IEnumerable<Release>> GetReleaseAsync(string artistId);
    }
}
