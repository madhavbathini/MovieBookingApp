using AutoMapper;
using Moq;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Models;
using MovieBookingApp.Repository;
using MovieBookingApp.Services;
using MovieBookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBookingApp.Tests.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ITokenGenerator> _tokenGeneratorMock;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();
            _tokenGeneratorMock = new Mock<ITokenGenerator>();
            _userService = new UserService(_userRepositoryMock.Object, _mapperMock.Object, _tokenGeneratorMock.Object);
        }

        [Test]
        public async Task AddUser_NewUser_ReturnsUserId()
        {
            // Arrange
            var userDto = new UserDto { /* Provide valid user properties */ };
            var userModel = new User { Id = "user_id" };

            _userRepositoryMock.Setup(r => r.GetUser(userDto.LoginId, userDto.Email)).ReturnsAsync((User)null);
            _mapperMock.Setup(m => m.Map<User>(userDto)).Returns(userModel);
            _userRepositoryMock.Setup(r => r.AddUser(userModel)).ReturnsAsync(true);

            // Act
            var result = await _userService.AddUser(userDto);

            // Assert
            Assert.That(result, Is.EqualTo(userModel.Id));
        }

        [Test]
        public async Task AddUser_ExistingUser_ReturnsEmptyUserId()
        {
            // Arrange
            var userDto = new UserDto { /* Provide valid user properties */ };
            var existingUser = new User { Id = "existing_user_id" };

            _userRepositoryMock.Setup(r => r.GetUser(userDto.LoginId, userDto.Email)).ReturnsAsync(existingUser);

            // Act
            var result = await _userService.AddUser(userDto);

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task GetUserToken_ValidCredentials_ReturnsUserTokenResponse()
        {
            // Arrange
            var loginId = "valid_login_id";
            var password = "valid_password";
            var existingUser = new User { /* Provide valid user properties */ };
            var token = "generated_token";
            var expectedResponse = new UserTokenResponse { User = existingUser, Token = token };

            _userRepositoryMock.Setup(r => r.GetUserByLoginIdPassword(loginId, password)).ReturnsAsync(existingUser);
            _tokenGeneratorMock.Setup(t => t.CreateToken(existingUser)).Returns(token);

            // Act
            var result = await _userService.GetUserToken(loginId, password);

            // Assert
            Assert.That(result.User, Is.EqualTo(expectedResponse.User));
            Assert.That(result.Token, Is.EqualTo(expectedResponse.Token));
        }

        [Test]
        public async Task GetUserToken_InvalidCredentials_ReturnsEmptyUserTokenResponse()
        {
            // Arrange
            var loginId = "invalid_login_id";
            var password = "invalid_password";

            _userRepositoryMock.Setup(r => r.GetUserByLoginIdPassword(loginId, password)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserToken(loginId, password);

            // Assert
            Assert.IsNull(result.User);
            Assert.IsNull(result.Token);
        }

        [Test]
        public async Task ChangePassword_ValidLoginId_ReturnsSuccessStatus()
        {
            // Arrange
            var loginId = "valid_login_id";
            var newPassword = "new_password";
            var existingUser = new User { /* Provide valid user properties */ };

            _userRepositoryMock.Setup(r => r.GetUserByLoginId(loginId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.UpdateUser(existingUser)).ReturnsAsync(true);

            // Act
            var result = await _userService.ChangePassword(loginId, newPassword);

            // Assert
            Assert.That(result, Is.EqualTo("Password changed successfully"));
        }

        [Test]
        public async Task ChangePassword_InvalidLoginId_ReturnsEmptyStatus()
        {
            // Arrange
            var loginId = "invalid_login_id";
            var newPassword = "new_password";

            _userRepositoryMock.Setup(r => r.GetUserByLoginId(loginId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.ChangePassword(loginId, newPassword);

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task ChangePassword_ExceptionThrown_ReturnsEmptyStatus()
        {
            // Arrange
            var loginId = "valid_login_id";
            var newPassword = "new_password";
            var existingUser = new User { /* Provide valid user properties */ };

            _userRepositoryMock.Setup(r => r.GetUserByLoginId(loginId)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.UpdateUser(existingUser)).Throws(new Exception());

            // Act
            var result = await _userService.ChangePassword(loginId, newPassword);

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }

}
