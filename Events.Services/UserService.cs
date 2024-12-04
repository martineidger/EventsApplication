using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.DTOs.DTOs;
using Events.Persistence.Repositories;
using Events.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _db;
        public UserService(IUnitOfWork unitOfWork)
        {
            _db = unitOfWork;
        }
        public User GetByLogin(string email, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsAlreadyExists(string email)
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(UserDTO userModel)
        {
            throw new NotImplementedException();
        }
    }
}
