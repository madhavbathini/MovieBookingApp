using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingApp.Filters;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Services;

namespace MovieBookingApp.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/moviebooking")]
    [ApiController]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost("AddMovie"), AllowAnonymous]
        [ServiceFilter(typeof(NullCheckFilter))]
        public async Task<ActionResult> AddMovie(Movie movie)
        {
            try
            {
                var movieId = await _movieService.AddMovie(movie);

                if (!string.IsNullOrEmpty(movieId))
                {
                    //_logger.LogInformation("Movie added successfully: {MovieId}", movieId);
                    return Created("", movieId);
                }
                else
                {
                    return BadRequest("Movie already exists");
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while adding the movie.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("All"),AllowAnonymous]
        public async Task<ActionResult<List<MovieDto>>> ViewAllMovies()
        {
            try
            {
                List<MovieDto>? movies = await _movieService.GetMovies();

                if (movies is not null && movies.Count > 0)
                {
                   // _logger.LogInformation("Retrieved all movies successfully");
                    return Ok(movies);
                }
                else
                {
                  //  _logger.LogInformation("No movies found");
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "An error occurred while retrieving movies");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Movies/Search/{movieId}"), AllowAnonymous]
        public async Task<ActionResult<MovieDto>> GetMovieById(string movieId)
        {
            try
            {
                var movie = await _movieService.GetMovieById(movieId);

                if (movie != null)
                {
                    //_logger.LogInformation("Movie found. Movie ID: {MovieId}", movieId);
                    return Ok(movie);
                }
                else
                {
                   // _logger.LogInformation("No movie found for the ID: {MovieId}", movieId);
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "An error occurred while retrieving the movie. Movie ID: {MovieId}", movieId);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Movies/Search/MovieName"), AllowAnonymous]
        public async Task<ActionResult<MovieDto>> SearchMovie(string movieName)
        {
            try
            {
                var movies = await _movieService.SearchMovie(movieName);

                if (movies is not null && movies.Count > 0)
                {
                    //_logger.LogInformation("Movie search successful. Movie name: {MovieName}", movieName);
                    return Ok(movies);
                }
                else
                {
                    //_logger.LogInformation("No movies found for the search query: {MovieName}", movieName);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "An error occurred while searching for movies. Movie name: {MovieName}", movieName);
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPatch("{moviename}/update/{ticket}")]
        public async Task<IActionResult> UpdateMovieTicketStatus(string moviename, string ticket)
        {
            try
            {
                TicketStatusResponse response = await _movieService.UpdateMovieTicketStatus(moviename, ticket);

                if (response.IsSuccess)
                {
                    // _logger.LogInformation("Movie ticket status updated for movie: {MovieName}", moviename);
                    return Ok(response);
                }
                else
                {
                    // _logger.LogInformation("Failed to update movie ticket status for movie: {MovieName}", moviename);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "An error occurred while updating movie ticket status for movie: {MovieName}", moviename);
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpDelete("{moviename}/delete/{id}")]
        public async Task<IActionResult> DeleteMovieById(string moviename, string id)
        {
            try
            {
                DeleteMovieByNameAndIdResponse response = await _movieService.DeleteMovieByNameAndId(moviename, id);

                if (response.IsSuccess)
                {
                    //_logger.LogInformation("Movie {MovieName} with ID {MovieId} deleted successfully", moviename, id);
                    return Ok(response);
                }
                else
                {
                    //_logger.LogInformation("Failed to delete movie {MovieName} with ID {MovieId}", moviename, id);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while deleting movie {MovieName} with ID {MovieId}", moviename, id);
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
