using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Controllers
{
    [ApiController]
    [Route("api/search")]
    [Produces("application/json")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        /// <summary>
        /// Get customer, product and order information from a customer id search term.
        /// </summary>
        /// <param name="term">The search term.</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested information.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchAsync([FromBody]SearchTerm term)
        {
            var result = await searchService.SearchAysnc(term.CustomerId);
            if (result.IsSuccess)
            {
                return Ok(result.SearchResult);
            }
            return NotFound();
        }
    }
}
