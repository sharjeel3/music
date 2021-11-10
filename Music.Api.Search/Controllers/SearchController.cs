using Microsoft.AspNetCore.Mvc;
using Music.Api.Search.Interfaces;
using System.Threading.Tasks;

namespace Music.Api.Search.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        /// <summary>
        /// Get a list of artists by an artist name search.
        /// </summary>
        /// <param name="artistName"></param>
        /// <returns>Artists</returns>
        [HttpGet("{artistName}")]
        public async Task<IActionResult> GetArtistsAsync(string artistName)
        {
            var result = await searchService.SearchArtistsAsync(artistName);

            if (result.SearchResults == null)
            {
                return NotFound(new
                {
                    Message = result.Message
                });
            }

            return Ok(new
            {
                Results = result.SearchResults,
                Message = result.Message
            });
        }
    }
}
