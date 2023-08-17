using NUnit.Framework;
using Moq;
using AutoMapper;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Repository;
using MovieBookingApp.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingApp.Tests.Services
{
    [TestFixture]
    public class MovieServiceTests
    {
        private MovieService _movieService;
        private Mock<IMovieRepository> _mockMovieRepository;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Movie, MovieDto>();
                cfg.CreateMap<MovieDto, Movie>();
            });
            _mapper = mapperConfig.CreateMapper();
            _movieService = new MovieService(_mockMovieRepository.Object, _mapper);
        }

        [Test]
        public async Task AddMovie_ValidMovie_ReturnsNonEmptyMovieId()
        {
            // Arrange
            var movie = new Movie { Name = "Test Movie", ReleaseDate = "22/06/2023" };

            // Mock the repository method
            _mockMovieRepository.Setup(repo => repo.AddMovie(It.IsAny<Movie>())).ReturnsAsync(true);

            // Act
            var result = await _movieService.AddMovie(movie);

            // Assert
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task AddMovie_InvalidMovie_ReturnsEmptyMovieId()
        {
            // Arrange
            var movie = new Movie(); // Invalid movie object

            // Mock the repository method
            _mockMovieRepository.Setup(repo => repo.AddMovie(It.IsAny<Movie>())).ReturnsAsync(false);

            // Act
            var result = await _movieService.AddMovie(movie);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetMovies_RepositoryReturnsMovies_ReturnsListOfMovieDtos()
        {
            // Arrange
            var moviesModel = new List<Movie>
            {
                new Movie {  Name = "Movie 1", ReleaseDate = "22/06/2023" },
                new Movie {  Name = "Movie 2", ReleaseDate = "2/06/2022"}
            };

            // Mock the repository method
            _mockMovieRepository.Setup(repo => repo.GetMovies()).ReturnsAsync(moviesModel);

            // Act
            var result = await _movieService.GetMovies();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Movie 1"));
            Assert.That(result[1].Name, Is.EqualTo("Movie 2"));
        }


        [Test]
        public async Task GetMovieById_InvalidMovieId_ThrowsException()
        {
            // Arrange
            var movieId = "invalid_movie_id";

            _mockMovieRepository.Setup(r => r.GetMovieById(movieId)).ThrowsAsync(new Exception());

            // Act and Assert
            Assert.ThrowsAsync<Exception>(async () => await _movieService.GetMovieById(movieId));
        }

        [Test]
        public async Task SearchMovie_ValidMovieName_ReturnsListOfMovieDtos()
        {
            // Arrange
            var movieName = "valid_movie_name";
            var moviesModel = new List<Movie>();
            var expectedMoviesDto = new List<MovieDto>();

            _mockMovieRepository.Setup(r => r.SearchMovie(movieName)).ReturnsAsync(moviesModel);
            

            // Act
            var result = await _movieService.SearchMovie(movieName);

            // Assert
            Assert.AreEqual(expectedMoviesDto, result);
        }

        [Test]
        public async Task SearchMovie_MovieNameNotFound_ReturnsEmptyList()
        {
            // Arrange
            var movieName = "nonexistent_movie_name";
            var moviesModel = new List<Movie>();

            _mockMovieRepository.Setup(r => r.SearchMovie(movieName)).ReturnsAsync(moviesModel);

            // Act
            var result = await _movieService.SearchMovie(movieName);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task UpdateMovieTicketStatus_ValidInputs_ReturnsSuccessfulResponse()
        {
            // Arrange
            var movieName = "valid_movie_name";
            var ticket = "valid_ticket";
            var expectedResponse = new TicketStatusResponse { IsSuccess = true, Message = "Movie ticket status updated" };

            _mockMovieRepository.Setup(r => r.UpdateTicket(movieName, ticket)).ReturnsAsync(true);

            // Act
            var result = await _movieService.UpdateMovieTicketStatus(movieName, ticket);

            // Assert
            Assert.AreEqual(expectedResponse.IsSuccess, result.IsSuccess);
            Assert.AreEqual(expectedResponse.Message, result.Message);
        }

        // Add test methods for UpdateMovieTicketStatus failure scenario and other edge cases

        [Test]
        public async Task DeleteMovieByNameAndId_ValidInputs_ReturnsSuccessfulResponse()
        {
            // Arrange
            var movieName = "valid_movie_name";
            var id = "valid_id";
            var expectedResponse = new DeleteMovieByNameAndIdResponse { IsSuccess = true, Message = $"Movie {movieName} deleted successfully" };

            _mockMovieRepository.Setup(r => r.DeleteMovie(id)).ReturnsAsync(true);

            // Act
            var result = await _movieService.DeleteMovieByNameAndId(movieName, id);

            // Assert
            Assert.AreEqual(expectedResponse.IsSuccess, result.IsSuccess);
            Assert.AreEqual(expectedResponse.Message, result.Message);
        }




    }
}
