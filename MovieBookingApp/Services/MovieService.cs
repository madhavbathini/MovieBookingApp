using AutoMapper;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Repository;

namespace MovieBookingApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }


        public async Task<string> AddMovie(Movie movie)
        {
            string movieId = string.Empty;

            try
            {

                movieId = Guid.NewGuid().ToString();
                var movieModel = _mapper.Map<Movie>(movie);
                movieModel.Id = movieId;

                var isInserted = await _movieRepository.AddMovie(movieModel);

                if (!isInserted)
                {
                    movieId = string.Empty;
                }

            }
            catch (Exception)
            {
                movieId = string.Empty;
            }

            return movieId;
        }

        public async Task<List<MovieDto>> GetMovies()
        {
            List<MovieDto> moviesView;
            try
            {
                var moviesModel = await _movieRepository.GetMovies();
                moviesView = _mapper.Map<List<MovieDto>>(moviesModel);
            }
            catch (Exception)
            {
                moviesView = new();
            }

            return moviesView;
        }
        public async Task<MovieDto> GetMovieById(string movieId)
        {
            try
            {
                var movieModel = await _movieRepository.GetMovieById(movieId);
                var movieDto = _mapper.Map<MovieDto>(movieModel);
                return movieDto;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the movie.", ex);
            }
        }

        public async Task<List<MovieDto>> SearchMovie(string movieName)
        {
            List<MovieDto> moviesView;
            try
            {
                var moviesModel = await _movieRepository.SearchMovie(movieName);
                moviesView = _mapper.Map<List<MovieDto>>(moviesModel);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for movies.", ex);

            }

            return moviesView;
        }

        public async Task<TicketStatusResponse> UpdateMovieTicketStatus(string moviename, string ticket)
        {
            TicketStatusResponse response = new TicketStatusResponse();


            response.IsSuccess = true;
            response.Message = "Movie ticket status updated";

            var isUpdateSuccess = await _movieRepository.UpdateTicket(moviename, ticket);

            if (isUpdateSuccess)
            {
                response.Message = "Movie ticket status updated";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Unable to update status ";
            }

            return response;
        }

        public async Task<DeleteMovieByNameAndIdResponse> DeleteMovieByNameAndId(string moviename, string id)
        {
            DeleteMovieByNameAndIdResponse response = new DeleteMovieByNameAndIdResponse();
            response.IsSuccess = true;
            response.Message = $"Movie {moviename} deleted successfully";

            try
            {

                var result = await _movieRepository.DeleteMovie(id);
                if (!result)
                {
                    response.IsSuccess = false;
                    response.Message = $"Movie with {id} Not Found, Please enter valid Id";
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Exception Occurs : " + ex.Message;
            }

            return response;
        }

    }
}
