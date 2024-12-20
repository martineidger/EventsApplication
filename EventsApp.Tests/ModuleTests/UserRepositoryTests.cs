﻿using Events.Authentications.AuthModels;
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
        public async Task Login_ValidCredentials_ReturnsTokens()
        {
            // Arrange
            var model = new AuhorizationModel { Email = "test@example.com", Password = "Password123!" };
            var tokenRequest = new GetTokenRequestModel { Email = testUser.Email, Role = testUser.Role, Id = testUser.Id };
            var tokens = new TokenResponse { AccessToken = "accessToken", RefreshToken = "refershToken"};

            _mockMapper.Setup(m => m.Map<GetTokenRequestModel>(testUser)).Returns(tokenRequest);
            _mockTokenService.Setup(t => t.GenerateTokens(tokenRequest)).Returns(tokens);

            // Act
            var result = await _userRepository.LoginAsync(model);

            // Assert
            Assert.Equal(tokens.AccessToken, result.AccessToken);
            Assert.Equal(tokens.RefreshToken, result.RefreshToken);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ThrowsException()
        {
            // Arrange
            var model = new AuhorizationModel { Email = "unknown@example.com", Password = "Password123!" };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await _userRepository.LoginAsync(model));
        }

        [Fact]
        public async Task Register_EmailAlreadyExists_ReturnsFalse()
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
            var result = await _userRepository.RegisterAsync(model);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Register_ValidUser_AddsUserAndReturnsTrue()
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
            var result = await _userRepository.RegisterAsync(model);

            // Assert
            Assert.True(result);
            Assert.Contains(_context.Users, u => u.Email == newUser.Email);
        }

        /*[Fact]
        public async Task SetRefreshToken_ValidUser_UpdatesToken()
        {
            // Arrange
            var email = "test@example.com";
            var refreshToken = "newRefreshToken";

            // Act
           await _userRepository.SetRefreshTokenAsync(refreshToken, email);

            // Assert
            var updatedUser = _context.Users.First(u => u.Email == email);
            Assert.Equal(refreshToken, updatedUser.Token);
        }*/

        [Fact]
        public async Task GetUserByName_UserExists_ReturnsUser()
        {
            // Arrange
            var userName = "John";

            // Act
            var result = await _userRepository.GetUserByNameAsync(userName);

            // Assert
            Assert.Equal(testUser.Id, result.Id);
        }

        [Fact]
        public async Task GetUserByName_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userName = "Unknown";

            // Act
            var result = await _userRepository.GetUserByNameAsync(userName);

            // Assert
            Assert.Null(result);
        }
    }
}

