using Events.Authentications.AuthModels;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.Persistence.DataBase;
using Events.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using AutoMapper;
using Events.Authentications.Services.Intrfaces;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace EventsApp.Tests.ModuleTests
{
    public class UserRepositoryTests
    {
        private readonly UserRepository _userRepository;
        private readonly AppDbContext _context;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAdminUserService> _mockAdminUserService;
        private readonly User testUser;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _mockTokenService = new Mock<ITokenService>();
            _mockMapper = new Mock<IMapper>();
            _mockAdminUserService = new Mock<IAdminUserService>();
            _userRepository = new UserRepository(_context, _mockTokenService.Object, _mockMapper.Object, _mockAdminUserService.Object);

            testUser = new User()
            {
                Email = "test@example.com",
                Password = "Password123!",
                Name = "John",
                Surname = "Doe",
                Role = "User",
                BirthDate = new DateOnly(1990, 1, 1),
                RegistrationDate = DateTime.UtcNow            
            };

            _context.Users.Add(testUser);
            _context.SaveChanges();
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsTokens()
        {
            // Arrange
            var model = new AuhorizationModel { Email = "test@example.com", Password = "Password123!" };
            var tokenRequest = new GetTokenRequestModel { Email = testUser.Email, Role = testUser.Role, Id = testUser.Id };
            var tokens = ("accessToken", "refreshToken");

            _mockMapper.Setup(m => m.Map<GetTokenRequestModel>(testUser)).Returns(tokenRequest);
            _mockTokenService.Setup(t => t.GenerateTokens(tokenRequest)).Returns(tokens);

            // Act
            var result = _userRepository.Login(model);

            // Assert
            Assert.Equal(tokens.Item1, result.accessToken);
            Assert.Equal(tokens.Item2, result.refreshToken);
        }

        [Fact]
        public void Login_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var model = new AuhorizationModel { Email = "unknown@example.com", Password = "Password123!" };

            // Act & Assert
            Assert.Throws<Exception>(() => _userRepository.Login(model));
        }

        [Fact]
        public void Register_EmailAlreadyExists_ReturnsFalse()
        {
            // Arrange
            var model = new RegistrationModel
            {
                Email = "existing@example.com",
                Password = "Password123!",
                Name = "John",
                Surname = "Doe",
                BirthDate = new DateOnly(1990, 1, 1),
            };

            var user = new User
            {
                Email = "existing@example.com",
                Password = "Password123!",
                Name = "John",
                Surname = "Doe",
                BirthDate = new DateOnly(1990, 1, 1),
                RegistrationDate = DateTime.UtcNow,
                Role = "User"
            };


            _context.Users.Add(user);
            _context.SaveChanges();

            // Act
            var result = _userRepository.Register(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Register_ValidUser_AddsUserAndReturnsTrue()
        {
            // Arrange
            var model = new RegistrationModel
            {
                Email = "nonexisting@example.com",
                Password = "Password123!",
                Name = "John",
                Surname = "Doe",
                BirthDate = new DateOnly(1990, 1, 1),
            };

            var newUser = new User
            {
                Email = "nonexisting@example.com",
                Password = "Password123!",
                Name = "John",
                Surname = "Doe",
                BirthDate = new DateOnly(1990, 1, 1),
                RegistrationDate = DateTime.UtcNow,
                Role = "User"
            };


            _mockMapper.Setup(m => m.Map<User>(model)).Returns(newUser);
            _mockAdminUserService.Setup(s => s.GetRole(newUser.Email)).Returns("User");

            // Act
            var result = _userRepository.Register(model);

            // Assert
            Assert.True(result);
            Assert.Contains(_context.Users, u => u.Email == newUser.Email);
        }

        [Fact]
        public void SetRefreshToken_ValidUser_UpdatesToken()
        {
            // Arrange
            var email = "test@example.com";
            var refreshToken = "newRefreshToken";

            // Act
            _userRepository.SetRefreshToken(refreshToken, email);

            // Assert
            var updatedUser = _context.Users.First(u => u.Email == email);
            Assert.Equal(refreshToken, updatedUser.Token);
        }

        [Fact]
        public void GetUserByName_UserExists_ReturnsUser()
        {
            // Arrange
            var userName = "John";

            // Act
            var result = _userRepository.GetUserByName(userName);

            // Assert
            Assert.Equal(testUser, result);
        }

        [Fact]
        public void GetUserByName_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userName = "Unknown";

            // Act
            var result = _userRepository.GetUserByName(userName);

            // Assert
            Assert.Null(result);
        }
    }
}

