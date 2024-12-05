using Events.Authentications.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.Services.Intrfaces
{
    public interface ITokenService 
    {
        public TokenResponse GenerateTokens(GetTokenRequestModel model);
        public TokenResponse RefreshTokens(string refreshToken, GetTokenRequestModel model);
    }
}
