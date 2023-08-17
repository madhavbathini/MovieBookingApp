using MovieBookingApp.Models;

namespace MovieBookingApp.Repository
{
    public interface ITicketRepository
    {
        public Task<bool> AddTicket(Ticket ticket);
        public Task<List<Ticket>> GetTickets(string userid);
    }
}
