using AutoMapper;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Models;
using MovieBookingApp.Repository;

namespace MovieBookingApp.Services
{
    public class TicketService:ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        public TicketService(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        public async Task<string> AddTicket(TicketDto ticket)
        {
            string status = string.Empty;
            try
            {
                var ticketModel = _mapper.Map<Ticket>(ticket);
                var isTicketInserted = await _ticketRepository.AddTicket(ticketModel);

                if (isTicketInserted)
                {
                    status = "Ticket booked successfully";
                }
                else
                {
                    status = "Ticket booking failed";
                }

            }
            catch (Exception)
            {
                status = string.Empty;
            }

            return status;
        }

        public async Task<List<TicketDto>> GetTickets(string userid)
        {
            List<TicketDto> ticketList;
           
            try
            {
                var ticketsModel = await _ticketRepository.GetTickets(userid);
                ticketList = _mapper.Map<List<TicketDto>>(ticketsModel);
            }
            catch (Exception)
            {
                ticketList = new();
            }

            return ticketList;
        }
    }
}
