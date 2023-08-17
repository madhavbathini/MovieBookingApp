using Microsoft.IdentityModel.Tokens;
using MovieBookingApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieBookingApp.Utilities
{
    public class TokenGenerator:ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(User user)
        {
            

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.LoginId)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

             var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }
    }
}
