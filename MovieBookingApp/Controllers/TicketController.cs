using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Services;

namespace MovieBookingApp.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/moviebooking")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
           
        }

        

        [HttpPost("{moviename}/bookticket")]
        public async Task<ActionResult<string>> AddTickets(TicketDto ticket)
        {
            try
            {
                var status = await _ticketService.AddTicket(ticket);

                if (!string.IsNullOrEmpty(status))
                {
                    //_logger.LogInformation("Ticket added successfully for movie: {MovieName}", ticket.MovieName);
                    return Ok(status);
                }
                else
                {
                    //_logger.LogInformation("Failed to add ticket for movie: {MovieName}", ticket.MovieName);
                    return BadRequest(status);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while adding ticket for movie: {MovieName}", ticket.MovieName);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{userid}/gettickets")]
        public async Task<ActionResult<List<TicketDto>>> GetTickets(string userid)
        {
            try
            {
                var tickets = await _ticketService.GetTickets(userid);
                if (tickets.Count!=0)
                {
                    //_logger.LogInformation("Ticket added successfully for movie: {MovieName}", ticket.MovieName);
                    return Ok(tickets);
                }
                else
                {
                    //_logger.LogInformation("Failed to add ticket for movie: {MovieName}", ticket.MovieName);
                    return BadRequest(tickets);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

        }

        
    }
}
