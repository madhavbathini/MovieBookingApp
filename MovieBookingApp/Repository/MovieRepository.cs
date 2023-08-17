using MongoDB.Bson;
using MongoDB.Driver;
using MovieBookingApp.Models;

namespace MovieBookingApp.Repository
{
    public class MovieRepository:IMovieRepository
    {
        private readonly IMongoCollection<Movie> _movies;

        private readonly IMongoDbContext _dbContext;
        public MovieRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;

            _movies = _dbContext.movies;
        }

        public async Task<List<Movie>> GetMovies()
        {
            var movies = await _movies.FindAsync(movie => true);
            return movies.ToList();
        }

        public async Task<Movie> GetMovieById(string movieId)
        {
            var filter = Builders<Movie>.Filter.Eq("Id", movieId);
            return await _movies.Find(filter).FirstOrDefaultAsync();
        }


        public async Task<List<Movie>> SearchMovie(string movieName)
        {
            var movies = await _movies.FindAsync<Movie>(
                Builders<Movie>.Filter.Regex("name", new BsonRegularExpression(movieName, "i")));

            return movies.ToList();
        }

        public async Task<bool> UpdateTicket(string movieName, string ticket)
        {
            var Filter = new BsonDocument()
                    .Add("ticketStatus", ticket);

            var updateDoc = new BsonDocument("$set", Filter);

            try
            {
                await _movies.UpdateOneAsync(x => x.Name == movieName, updateDoc);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteMovie(string id)
        {
            var result =await _movies.DeleteOneAsync(x=>x.Id==id);
            return result.IsAcknowledged;
        }

        public async Task<bool> AddMovie(Movie movieModel)
        {
            try
            {
                await _movies.InsertOneAsync(movieModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
