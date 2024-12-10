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
        /*private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IAdminUserService _adminUserService;*/
        public UserRepository(AppDbContext context/*, ITokenService tokenService, IMapper mapper, IAdminUserService adminUserService*/) : base(context)
        {
            _context = context;
            /*_tokenService = tokenService;
            _mapper = mapper;
            _adminUserService = adminUserService;*/
        }

        /*public async Task<TokenResponse> LoginAsync(AuhorizationModel model)
        {
            var user = await  _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            if(user == null)
            {
                throw new Exception("Invalid credentials");
            }

            var tokenRequest = _mapper.Map<GetTokenRequestModel>(user);
            var tokens = _tokenService.GenerateTokens(tokenRequest);

            return tokens;
        }

        public async Task<bool> RegisterAsync(RegistrationModel user)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(u => u.Email.Equals(user.Email)))
                return false;

            var newUser = _mapper.Map<User>(user);

            newUser.Role = _adminUserService.GetRole(newUser.Email);
 
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return true;
        }*/
        /*public async Task SetRefreshTokenAsync(string refreshToken, string email)
        {
            var cUser = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
            cUser.Token = refreshToken; 
            await _context.SaveChangesAsync();
        }*/

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
