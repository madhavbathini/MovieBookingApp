using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;

namespace MovieBookingApp.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly IMongoCollection<User> _users;

        private readonly IMongoDbContext _dbContext;
        public UserRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;

            _users = _dbContext.users;
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                await _users.InsertOneAsync(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task DeleteUser(string id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }

        public async Task<User> GetUser(string id)
        {
            var users = await _users.FindAsync(user => user.Id == id);
            return await users.FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(string loginId, string email)
        {
            var users = await _users.FindAsync(user => user.LoginId == loginId || user.Email == email);
            return await users.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByLoginIdPassword(string loginId, string password)
        {
            var users = await _users.FindAsync(user => user.LoginId == loginId && user.Password == password);
            return await users.FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByLoginId(string loginId)
        {
            var users = await _users.FindAsync(user => user.LoginId.Equals(loginId));
            return await users.FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _users.FindAsync(user => true);
            return users.ToList();
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
