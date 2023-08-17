using AutoMapper;
using Moq;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Models;
using MovieBookingApp.Repository;
using MovieBookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBookingApp.Tests.Services
{

    [TestFixture]
    public class TicketServiceTests
    {
        private TicketService _ticketService;
        private Mock<ITicketRepository> _ticketRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _mapperMock = new Mock<IMapper>();
            _ticketService = new TicketService(_ticketRepositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task AddTicket_ValidTicketDto_ReturnsSuccessStatus()
        {
            // Arrange
            var ticketDto = new TicketDto {  };
            var ticketModel = new Ticket();

            _mapperMock.Setup(m => m.Map<Ticket>(ticketDto)).Returns(ticketModel);
            _ticketRepositoryMock.Setup(r => r.AddTicket(ticketModel)).ReturnsAsync(true);

            // Act
            var result = await _ticketService.AddTicket(ticketDto);

            // Assert
            Assert.That(result, Is.EqualTo("Ticket booked successfully"));
        }

        [Test]
        public async Task AddTicket_InvalidTicketDto_ReturnsFailureStatus()
        {
            // Arrange
            var ticketDto = new TicketDto {  };
            var ticketModel = new Ticket();

            _mapperMock.Setup(m => m.Map<Ticket>(ticketDto)).Returns(ticketModel);
            _ticketRepositoryMock.Setup(r => r.AddTicket(ticketModel)).ReturnsAsync(false);

            // Act
            var result = await _ticketService.AddTicket(ticketDto);

            // Assert
            Assert.That(result, Is.EqualTo("Ticket booking failed"));
        }

        [Test]
        public async Task AddTicket_ExceptionThrown_ReturnsEmptyStatus()
        {
            // Arrange
            var ticketDto = new TicketDto {  };

            _mapperMock.Setup(m => m.Map<Ticket>(ticketDto)).Throws(new Exception());

            // Act
            var result = await _ticketService.AddTicket(ticketDto);

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }


}
