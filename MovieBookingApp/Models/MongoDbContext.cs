
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MovieBookingApp.Models
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IOptions<MongoDBConnection> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<Movie> movies => _database.GetCollection<Movie>("movies");
        public IMongoCollection<User> users => _database.GetCollection<User>("users");
        public IMongoCollection<Ticket> tickets => _database.GetCollection<Ticket>("tickets");
    }
}
