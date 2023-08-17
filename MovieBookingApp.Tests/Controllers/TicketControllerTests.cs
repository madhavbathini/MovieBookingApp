using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieBookingApp.Controllers;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Models;
using MovieBookingApp.Services;
using System;


namespace MovieBookingApp.Tests.Controllers
{
    public class TicketControllerTests
    {
        private TicketController _controller;
        private Mock<ITicketService> _ticketServiceMock;
        

        [SetUp]
        public void Setup()
        {
            _ticketServiceMock = new Mock<ITicketService>();
            _controller = new TicketController(_ticketServiceMock.Object);
        }

        [Test]
        public async Task AddTickets_ValidTicket_ReturnsOkResult()
        {
            // Arrange
            TicketDto ticket = new TicketDto
            {
                MovieName = "Movie 1",
                TheatreName = "Theatre 1",
                NumberOfTickets = 2,
                SeatNumber = "A1,A2"
            };

            _ticketServiceMock.Setup(mock => mock.AddTicket(ticket))
                .ReturnsAsync("Ticket booked successfully");

            // Act
            var result = await _controller.AddTickets(ticket);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));

            var status = okObjectResult.Value as string;
            Assert.NotNull(status);
            Assert.That(status, Is.EqualTo("Ticket booked successfully"));
        }

        [Test]
        public async Task AddTickets_TicketNotAdded_ReturnsBadRequest()
        {
            // Arrange
            var invalidTicket = new TicketDto
            {
                // Populate the ticket object with invalid data that would cause it to fail validation or Service rules
                // For example, missing required fields or invalid values
            };

            _ticketServiceMock.Setup(mock => mock.AddTicket(invalidTicket))
                .ReturnsAsync(string.Empty); // Return an empty string to indicate ticket not added successfully

            // Act
            var result = await _controller.AddTickets(invalidTicket);

            // Assert
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
            Assert.That(badRequestResult.Value, Is.EqualTo(string.Empty));
        }


        
    }
}
