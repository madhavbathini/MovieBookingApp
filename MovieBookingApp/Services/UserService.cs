using AutoMapper;
using MovieBookingApp.Models;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Repository;
using MovieBookingApp.Utilities;

namespace MovieBookingApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenGenerator _tokenGenerator;

        public UserService(IUserRepository userRepository, IMapper mapper, ITokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> AddUser(UserDto user)
        {
            string userId = string.Empty;

            try
            {
                var existingUser = await _userRepository.GetUser(user.LoginId, user.Email);

                if (existingUser is null || string.IsNullOrEmpty(existingUser?.Id))
                {

                    userId = Guid.NewGuid().ToString();
                    var userModel = _mapper.Map<User>(user);
                    userModel.Id = userId;
                    userModel.UserType = "CUSTOMER";

                    var isInserted = await _userRepository.AddUser(userModel);

                    if (!isInserted)
                    {
                        userId = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                userId = string.Empty;
            }

            return userId;
        }

        public async Task<UserTokenResponse> GetUserToken(string loginId, string password)
        {
            UserTokenResponse userTokenResponse = new UserTokenResponse();
            
            try
            {
                var existingUserModel = await _userRepository.GetUserByLoginIdPassword(loginId, password);
                if (existingUserModel is not null)
                {
                    userTokenResponse.User = existingUserModel;
                    userTokenResponse.Token = _tokenGenerator.CreateToken(existingUserModel);
                }
            }
            catch (Exception)
            {
                userTokenResponse = new UserTokenResponse();
            }

            return userTokenResponse;
        }

        public async Task<string> ChangePassword(string loginId, string newPassword)
        {
            string status = string.Empty;
            try
            {
                var existingUserModel = await _userRepository.GetUserByLoginId(loginId);
                if (existingUserModel is not null)
                {

                    existingUserModel.Password = newPassword;


                    var isUpdateSuccess = await _userRepository.UpdateUser(existingUserModel);

                    if (isUpdateSuccess)
                    {
                        status = "Password changed successfully";
                    }
                }
                else
                {
                    status = string.Empty;
                }
            }
            catch (Exception)
            {
                status = string.Empty;
            }

            return status;
        }
    }
}
