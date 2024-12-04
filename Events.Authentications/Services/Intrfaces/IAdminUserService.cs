using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.Services.Intrfaces
{
    public interface IAdminUserService
    {
        public List<string> GetAdminEmails();
        public bool IsAdmin(string email);
        public string GetRole(string email);
    }
}
