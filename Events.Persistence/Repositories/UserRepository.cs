using AutoMapper;
using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.Persistence.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Events.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IAdminUserService _adminUserService;
        public UserRepository(AppDbContext context, ITokenService tokenService, IMapper mapper, IAdminUserService adminUserService) : base(context)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _adminUserService = adminUserService;
        }

        public (string accessToken, string refreshToken) Login(AuhorizationModel model)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if(user == null)
            {
                throw new Exception("Invalid credentials");
            }

            var tokenRequest = _mapper.Map<GetTokenRequestModel>(user);
            var tokens = _tokenService.GenerateTokens(tokenRequest);

            return tokens;
        }

        public bool Register(RegistrationModel user)
        {
            if (_context.Users.Any(u => u.Email.Equals(user.Email)))
                return false;

            var newUser = _mapper.Map<User>(user);

            newUser.Role = _adminUserService.GetRole(newUser.Email);
 
            _context.Users.Add(newUser);
            _context.SaveChanges();

            return true;
        }
        public void SetRefreshToken(string refreshToken, string email)
        {
            var cUser = _context.Users.FirstOrDefault(u => u.Email == email);
            cUser.Token = refreshToken;
            _context.SaveChanges();
        }

        public User GetUserByName(string name)
        {
            return _context.Users.FirstOrDefault(u => u.Name == name);
        }        
    }
}
