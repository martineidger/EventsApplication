using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.Services
{
    public class AuthoService : IAuthoService
    {
        public void Login(AuhorizationModel user)
        {
            
        }

        void IAuthoService.RefreshToken(string token)
        {
            
        }
    }
}
