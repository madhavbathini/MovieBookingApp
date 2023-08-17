using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Models;

namespace MovieBookingApp.Services
{
    public interface IMovieService
    {
        public Task<List<MovieDto>> GetMovies();

        public Task<MovieDto> GetMovieById(string movieId);
        public Task<List<MovieDto>> SearchMovie(string movieName);

        public Task<TicketStatusResponse> UpdateMovieTicketStatus(string moviename, string ticket);

        public Task<DeleteMovieByNameAndIdResponse> DeleteMovieByNameAndId(string moviename, string id);
        public Task<string> AddMovie(Movie movie);
    }
}
