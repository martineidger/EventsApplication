using Events.Authentications.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.Services.Intrfaces
{
    public interface IAuthoService
    {
        public void Login(AuhorizationModel user);
        protected void RefreshToken(string token);
    }
}
