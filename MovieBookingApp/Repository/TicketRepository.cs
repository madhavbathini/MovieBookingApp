using MongoDB.Driver;
using MovieBookingApp.Models;

namespace MovieBookingApp.Repository
{
    public class TicketRepository:ITicketRepository
    {
        private readonly IMongoCollection<Ticket> _tickets;

        private readonly IMongoDbContext _dbContext;
        public TicketRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;

            _tickets = _dbContext.tickets;
        }

        public async Task<bool> AddTicket(Ticket ticket)
        {
            try
            {
                await _tickets.InsertOneAsync(ticket);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Ticket>> GetTickets(string userid)
        {
            
                var tickets=await _tickets.FindAsync<Ticket>(x => x.UserId == userid);
                return tickets.ToList();
            
        }
    }
}
