using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieBookingApp.Controllers;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBookingApp.Tests.Controllers
{
    public class MovieControllerTests
    {
        private MovieController _controller;
        private Mock<IMovieService> _movieServiceMock;

        [SetUp]
        public void Setup()
        {
            _movieServiceMock = new Mock<IMovieService>();
            _controller = new MovieController(_movieServiceMock.Object);
        }

        [Test]
        public async Task ViewAllMovies_ReturnsListOfMovies()
        {
            // Arrange
            List<MovieDto> expectedMovies = new List<MovieDto>
            {
                new MovieDto { Name = "Movie 1", TheatreName = "Theatre 1", IsAvailable = true },
                new MovieDto { Name = "Movie 2", TheatreName = "Theatre 2", IsAvailable = false }
            };

            _movieServiceMock.Setup(mock => mock.GetMovies())
                .ReturnsAsync(expectedMovies);

            // Act
            var result = await _controller.ViewAllMovies();

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));

            var movies = okObjectResult.Value as List<MovieDto>;
            Assert.NotNull(movies);
            Assert.That(movies.Count, Is.EqualTo(expectedMovies.Count));
        }
        [Test]
        public async Task ViewAllMovies_ReturnsNoContent_WhenNoMoviesExist()
        {
            // Arrange
            List<MovieDto> emptyMovies = new List<MovieDto>();

            _movieServiceMock.Setup(mock => mock.GetMovies())
                .ReturnsAsync(emptyMovies);

            // Act
            var result = await _controller.ViewAllMovies();

            // Assert
            var noContentResult = result.Result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }
        [Test]
        public async Task SearchMovie_ReturnsMatchingMovies()
        {
            // Arrange
            string movieName = "Movie 1";
            List<MovieDto> expectedMovies = new List<MovieDto>
            {
                new MovieDto { Name = "Movie 1", TheatreName = "Theatre 1", IsAvailable = true },
                new MovieDto { Name = "Movie 1 - Sequel", TheatreName = "Theatre 2", IsAvailable = false }
            };

            _movieServiceMock.Setup(mock => mock.SearchMovie(movieName))
                .ReturnsAsync(expectedMovies);

            // Act
            var result = await _controller.SearchMovie(movieName);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));

            var movies = okObjectResult.Value as List<MovieDto>;
            Assert.NotNull(movies);
            Assert.That(movies.Count, Is.EqualTo(expectedMovies.Count));
        }
        [Test]
        public async Task SearchMovie_NoMoviesFound_ReturnsNoContent()
        {
            // Arrange
            string movieName = "Invalid Movie";

            _movieServiceMock.Setup(mock => mock.SearchMovie(movieName))
                .ReturnsAsync(new List<MovieDto>());

            // Act
            var result = await _controller.SearchMovie(movieName);

            // Assert
            var noContentResult = result.Result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.That(noContentResult.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public async Task UpdateMovieTicketStatus_ValidTicket_ReturnsOkResult()
        {
            // Arrange
            string movieName = "Movie 1";
            string ticket = "Ticket 123";

            var response = new TicketStatusResponse
            {
                IsSuccess = true,
                Message = "Movie ticket status updated"
            };

            _movieServiceMock.Setup(mock => mock.UpdateMovieTicketStatus(movieName, ticket))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.UpdateMovieTicketStatus(movieName, ticket);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));

            var responseData = okObjectResult.Value as TicketStatusResponse;
            Assert.NotNull(responseData);
            Assert.That(responseData.IsSuccess, Is.EqualTo(response.IsSuccess));
            Assert.That(responseData.Message, Is.EqualTo(response.Message));
        }

        [Test]
        public async Task UpdateMovieTicketStatus_TicketStatusUpdateFailed_ReturnsBadRequest()
        {
            // Arrange
            var movieName = "Movie 1";
            var ticket = "ABC123";

            _movieServiceMock.Setup(mock => mock.UpdateMovieTicketStatus(movieName, ticket))
                .ReturnsAsync(new TicketStatusResponse { IsSuccess = false });

            // Act
            var result = await _controller.UpdateMovieTicketStatus(movieName, ticket);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));

            var response = badRequestResult.Value as TicketStatusResponse;
            Assert.NotNull(response);
            Assert.IsFalse(response.IsSuccess);
        }


        [Test]
        public async Task DeleteRecordById_ValidId_ReturnsOkResult()
        {
            // Arrange
            string movieName = "Movie 1";
            string id = "123";

            var response = new DeleteMovieByNameAndIdResponse
            {
                IsSuccess = true,
                Message = $"Movie {movieName} deleted successfully"
            };

            _movieServiceMock.Setup(mock => mock.DeleteMovieByNameAndId(movieName, id))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.DeleteMovieById(movieName, id);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));

            var responseData = okObjectResult.Value as DeleteMovieByNameAndIdResponse;
            Assert.NotNull(responseData);
            Assert.That(responseData.IsSuccess, Is.EqualTo(response.IsSuccess));
            Assert.That(responseData.Message, Is.EqualTo(response.Message));
        }


    }
}
