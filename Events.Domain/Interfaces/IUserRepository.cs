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
        public bool Register(RegistrationModel user);
        public (string accessToken, string refreshToken) Login(AuhorizationModel model);
        public User GetUserByName(string name);
        public void SetRefreshToken(string refreshToken, string email);
        
    }
}
