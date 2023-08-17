using MovieBookingApp.Models.Dtos;

namespace MovieBookingApp.Services
{
    public interface ITicketService
    {
        public Task<string> AddTicket(TicketDto ticket);
        public Task<List<TicketDto>> GetTickets(string userid);
    }
}
