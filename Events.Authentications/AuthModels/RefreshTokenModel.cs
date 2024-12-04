using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.AuthModels
{
    public class RefreshTokenModel
    {
        public string Token { get; set; }
        public string UserId {  get; set; }
        public string ExpiresIn { get; set; }
    }
}
