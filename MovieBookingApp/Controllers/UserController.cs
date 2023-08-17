using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MovieBookingApp.Filters;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Services;

namespace MovieBookingApp.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/moviebooking")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }

        [HttpPost("Register")]
        [ServiceFilter(typeof(NullCheckFilter))]
        public async Task<ActionResult> Register(UserDto user)
        {
            try
            {
                var userId = await _userService.AddUser(user);

                if (!string.IsNullOrEmpty(userId))
                {
                    //_logger.LogInformation("User registered successfully: {UserId}", userId);
                    return Created("", userId);
                }
                else
                {
                    return BadRequest("User already exists");
                }
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "An error occurred while registering the user.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("Login")]
        public async Task<ActionResult<UserTokenResponse>> Login(string loginId, string password)
        {
            try
            {
                var response = await _userService.GetUserToken(loginId, password);

                if (!string.IsNullOrEmpty(response.Token))
                { 
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Incorrect LoginId or Password");
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while logging in.");
                return StatusCode(500, "Internal server error");
            }
        }

       [HttpGet("{loginId}/Forgot")]
       public async Task<ActionResult<string>> Forgot(string loginId,string newPassword)
        {
            try
            {
                var passwordChangedStatus = await _userService.ChangePassword(loginId, newPassword);

                if (!string.IsNullOrEmpty(passwordChangedStatus))
                {
                    //_logger.LogInformation("Password changed successfully for user: {LoginId}", loginId);
                    return Ok(passwordChangedStatus);
                }
                else
                {
                    //_logger.LogWarning("Failed to change password for user: {LoginId}", loginId);
                    return BadRequest(passwordChangedStatus);
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while changing password for user: {LoginId}", loginId);
                return StatusCode(500, "Internal server error");
            }

        }

    }
}
