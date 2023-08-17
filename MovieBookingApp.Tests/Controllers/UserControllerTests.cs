

using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieBookingApp.Controllers;
using MovieBookingApp.Models.Dtos;
using MovieBookingApp.Models;
using MovieBookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieBookingApp.Tests.Controllers
{
    public class UserControllerTests
    {
        private UserController _controller;
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }


        [Test]
        public async Task Register_ValidUser_ReturnsCreatedResult()
        {
            // Arrange
            var userDto = new UserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                LoginId = "john.doe",
                Password = "password",
                Contact = "1234567890"
            };
            _userServiceMock.Setup(x => x.AddUser(userDto)).ReturnsAsync("12345");

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            Assert.IsInstanceOf<CreatedResult>(result);
            var createdResult = result as CreatedResult;
            Assert.That(actual: createdResult.Value, Is.EqualTo("12345"));
        }

        [Test]
        public async Task Register_ExistingUser_ReturnsBadRequest()
        {
            // Arrange
            var userDto = new UserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                LoginId = "john.doe",
                Password = "password",
                Contact = "1234567890"
            };
            _userServiceMock.Setup(x => x.AddUser(userDto)).ReturnsAsync(string.Empty);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            string loginId = "testuser";
            string password = "password123";
            string expectedToken = "sample_token";
            var existingUser = new User
            {
                Id = loginId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                LoginId = loginId,
                Password = password,
                Contact = "1234567890"

            };
            UserTokenResponse response = new UserTokenResponse()
            {
                User = existingUser,
                Token = expectedToken
            };

            _userServiceMock.Setup(mock => mock.GetUserToken(loginId, password))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Login(loginId, password);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));
            var userTokenResponse = okObjectResult.Value as UserTokenResponse;
            Assert.That(userTokenResponse.Token, Is.EqualTo(expectedToken));
        }

        [Test]
        public async Task Forgot_ValidLoginIdAndNewPassword_ReturnsPasswordChangedStatus()
        {
            // Arrange
            string loginId = "testuser";
            string newPassword = "newpassword";
            string expectedPasswordChangedStatus = "Password changed successfully";

            _userServiceMock.Setup(mock => mock.ChangePassword(loginId, newPassword))
                .ReturnsAsync(expectedPasswordChangedStatus);

            // Act
            var result = await _controller.Forgot(loginId, newPassword);

            // Assert
            var okObjectResult = result.Result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.That(okObjectResult.StatusCode, Is.EqualTo(200));
            Assert.That(okObjectResult.Value, Is.EqualTo(expectedPasswordChangedStatus));
        }
    }
}
