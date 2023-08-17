using MovieBookingApp.Models;

namespace MovieBookingApp.Utilities
{
    public interface ITokenGenerator
    {
        public string CreateToken(User user);
    }
}
