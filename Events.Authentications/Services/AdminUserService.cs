using Events.Authentications.Services.Intrfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.Services
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IConfiguration _configuration;
        public AdminUserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<string> GetAdminEmails()
        {
            return _configuration.GetSection("AdminEmails").Get<List<string>>();
        }

        public bool IsAdmin(string email)
        {
            return GetAdminEmails().Contains(email);
        }

        public string GetRole(string email)
        {
            return IsAdmin(email) ? "Admin" : "User";
        }
    }
}
