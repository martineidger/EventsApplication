using Events.Domain.Entities;
using Events.DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Services.Interfaces
{
    public interface IUserService
    {
        public bool IsAlreadyExists(string email);
        public User GetByLogin(string email, string password);
        public void RegisterUser(UserDTO userModel);
    }
}
