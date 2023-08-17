using MovieBookingApp.Models;

namespace MovieBookingApp.Repository
{
    public interface IMovieRepository
    {
        public Task<List<Movie>> GetMovies();
        public Task<Movie> GetMovieById(string movieId);
        public Task<List<Movie>> SearchMovie(string movieName);

        public Task<bool> UpdateTicket(string movieName, string ticket);
        public Task<bool> DeleteMovie(string id);
        public Task<bool> AddMovie(Movie movieModel);
    }
}
