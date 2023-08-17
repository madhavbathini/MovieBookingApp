using MongoDB.Driver;

namespace MovieBookingApp.Models
{
    public interface IMongoDbContext
    {
        IMongoCollection<Movie> movies { get; }
        IMongoCollection<User> users { get; }
        IMongoCollection<Ticket> tickets { get; }
    }
}
