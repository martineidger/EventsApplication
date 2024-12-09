using Events.Domain.Entities;
using Events.Authentications.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Events.Authentications.Services.Intrfaces;

namespace Events.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        //public bool Register(RegistrationModel user);
        //public TokenResponse Login(AuhorizationModel model);
        /*public User GetUserByName(string name);
        public void SetRefreshToken(string refreshToken, string email);*/
        public Task<TokenResponse> LoginAsync(AuhorizationModel model);
        public Task<User> GetByIdAsync(int id);
        public Task<User> GetUserByNameAsync(string name);
        public Task SetRefreshTokenAsync(string refreshToken, string email);
        public Task<bool> RegisterAsync(RegistrationModel user);

    }
}
