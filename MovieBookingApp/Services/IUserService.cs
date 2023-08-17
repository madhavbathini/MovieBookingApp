
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;

namespace MovieBookingApp.Services
{
    public interface IUserService
    {
        public Task<string> AddUser(UserDto user);
        public Task<UserTokenResponse> GetUserToken(string loginId, string password);
        public Task<string> ChangePassword(string loginId, string newPassword);
    }
}
